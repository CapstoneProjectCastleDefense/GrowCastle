namespace Runtime.Scenes.Popups
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.StaticValues;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class TierUpPopupModel
    {
        public TierUpPopupModel(string inventoryId, Action onTierUp)
        {
            this.InventoryId = inventoryId;
            this.OnTierUp    = onTierUp;
        }
        public string InventoryId { get; private set; }
        public Action OnTierUp    { get; private set; }
    }

    public class TierUpPopupView : BaseView
    {
        public Button ConfirmBtn;
        public Button CancelBtn;
        public Button SelectBtn;
        public Image  RarityImage;
        public Image  ItemImage;
    }

    [PopupInfo(nameof(TierUpPopupView), isOverlay: true)]
    public class TierUpPopupPresenter : BasePopupPresenter<TierUpPopupView, TierUpPopupModel>
    {
        private readonly IGameAssets                  gameAssets;
        private readonly ItemBlueprint                itemBlueprint;
        private readonly InventoryLocalDataController inventoryLocalDataController;
        private readonly IScreenManager               screenManager;
        public TierUpPopupPresenter(SignalBus signalBus, ILogService logService, IGameAssets gameAssets, ItemBlueprint itemBlueprint,
            InventoryLocalDataController inventoryLocalDataController, IScreenManager screenManager) : base(signalBus, logService)
        {
            this.gameAssets                   = gameAssets;
            this.itemBlueprint                = itemBlueprint;
            this.inventoryLocalDataController = inventoryLocalDataController;
            this.screenManager                = screenManager;
        }

        private ItemData materialItem;

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.SelectBtn.onClick.AddListener(this.SelectItem);
            this.View.ConfirmBtn.onClick.AddListener(this.Confirm);
            this.View.CancelBtn.onClick.AddListener(this.CloseView);
        }
        private void Confirm()
        {
            this.inventoryLocalDataController.RemoveItem(this.materialItem.InventoryId);
            this.inventoryLocalDataController.GetItem(this.Model.InventoryId).Tier++;
            this.materialItem = null;
            this.Model.OnTierUp();
            this.CloseView();
        }
        private void SelectItem()
        {
            this.screenManager.OpenScreen<ItemInventoryForTierUpPopupPresenter, ItemInventoryForTierUpPopupModel>(
                new(
                    this.inventoryLocalDataController.GetItem(this.Model.InventoryId),
                    this.OnSelectItem
                    )).Forget();
        }
        
        private void OnSelectItem(string inventoryId)
        {
            this.materialItem = this.inventoryLocalDataController.GetItem(inventoryId);
            this.BindVolatileData().Forget();
        }
        
        public override async UniTask BindData(TierUpPopupModel popupModel) { this.BindVolatileData().Forget(); }

        private async UniTaskVoid BindVolatileData()
        {
            this.View.RarityImage.sprite =
                await this.gameAssets.LoadAssetAsync<Sprite>(this.materialItem == null ? RarityEnum.Common.ToString() : this.materialItem.Rarity.ToString());
            this.View.ItemImage.sprite = await this.gameAssets.LoadAssetAsync<Sprite>(this.materialItem == null
                ? MiscValue.TransparentImage
                : this.itemBlueprint.GetDataById(this.materialItem.BlueprintId).ImageAddress);
            var itemData = this.inventoryLocalDataController.GetItem(this.Model.InventoryId);
            this.View.ConfirmBtn.interactable =
                this.materialItem != null
                && this.materialItem.Rarity == itemData.Rarity
                && this.materialItem.BlueprintId == itemData.BlueprintId
                && this.materialItem.Tier == itemData.Tier;
        }
    }
}