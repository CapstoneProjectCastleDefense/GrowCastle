namespace Runtime.Scenes.Popups
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class ItemDetailPopupModel
    {
        public ItemDetailPopupModel(ItemModel itemModel, IEquippable equippable, string inventoryId)
        {
            this.ItemModel   = itemModel;
            this.Equippable  = equippable;
            this.InventoryId = inventoryId;
        }
        public ItemModel   ItemModel   { get; set; }
        public IEquippable Equippable  { get; set; }
        public string      InventoryId { get; private set; }
    }

    public class ItemDetailPopupView : BaseView
    {
        public Button   CloseButton;
        public Button   EquipButton;
        public Button   LevelButton;
        public Button   TierButton;
        public Button   UnequipButton;
        public Image    ItemImage;
        public Image    RarityImage;
        public TMP_Text Description;
        public TMP_Text Quantity;
        public TMP_Text Level;
        public TMP_Text Tier;
    }

    [PopupInfo(nameof(ItemDetailPopupView), isCloseWhenTapOutside: false, isOverlay: true)]
    public class ItemDetailPopupPresenter : BasePopupPresenter<ItemDetailPopupView, ItemDetailPopupModel>
    {
        private readonly IGameAssets    gameAssets;
        private readonly IScreenManager screenManager;
        public ItemDetailPopupPresenter(SignalBus signalBus, ILogService logService, IGameAssets gameAssets, IScreenManager screenManager) : base(signalBus, logService)
        {
            this.gameAssets    = gameAssets;
            this.screenManager = screenManager;
        }
        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.EquipButton.onClick.AddListener(() => { this.Model.Equippable.Equip(this.Model.InventoryId); });
            this.View.UnequipButton.onClick.AddListener(() => { this.Model.Equippable.UnEquip(this.Model.InventoryId); });
            this.View.CloseButton.onClick.AddListener(this.CloseView);

            this.View.LevelButton.onClick.AddListener(() =>
            {
                this.screenManager.OpenScreen<LevelUpPopupPresenter, LevelUpPopupModel>(new(this.Model.InventoryId, this.OnLevelUp)).Forget();
            });

            this.View.TierButton.onClick.AddListener(() =>
            {
                this.screenManager.OpenScreen<TierUpPopupPresenter, TierUpPopupModel>(new(this.Model.InventoryId, this.OnTierUp)).Forget();
            });
        }

        private void OnLevelUp()
        {
            this.Model.ItemModel.Level++;
            this.BindVolatileData();
        }

        private void OnTierUp() { }

        public override async UniTask BindData(ItemDetailPopupModel popupModel)
        {
            this.Model                   = popupModel;
            this.View.RarityImage.sprite = await this.gameAssets.LoadAssetAsync<Sprite>(this.Model.ItemModel.Rarity.ToString());
            this.View.ItemImage.sprite   = await this.gameAssets.LoadAssetAsync<Sprite>(this.Model.ItemModel.AddressableName);
            this.BindVolatileData();
        }

        private void BindVolatileData()
        {
            this.View.Quantity.text = this.Model.ItemModel.Quantity.ToString();
            this.View.Description.text = this.Model.ItemModel.Stats is { Count: > 0 }
                ? this.Model.ItemModel.Stats
                    .Select(stat => $"{stat.Key}: +{stat.Value.Item2}")
                    .Aggregate((current, next) => $"{current}\n{next}")
                : "";
            var canTierUp = this.Model.ItemModel.Level == (this.Model.ItemModel.Tier + 1) * 10;
            this.View.Level.text = $"Level: {this.Model.ItemModel.Level}";
            this.View.Tier.text  = $"Tier: {this.Model.ItemModel.Tier}";
            this.View.EquipButton.gameObject.SetActive(
                this.Model.ItemModel.ItemType == ItemType.Equipment
                && this.Model.Equippable != null
                && !this.Model.ItemModel.IsEquipped);
            this.View.UnequipButton.gameObject.SetActive(
                this.Model.ItemModel.ItemType == ItemType.Equipment
                && this.Model.Equippable != null
                && this.Model.ItemModel.IsEquipped);
            this.View.LevelButton.gameObject.SetActive(this.Model.ItemModel.ItemType == ItemType.Equipment && !canTierUp);
            this.View.TierButton.gameObject.SetActive(this.Model.ItemModel.ItemType == ItemType.Equipment && canTierUp);
            this.View.Level.gameObject.SetActive(this.Model.ItemModel.ItemType == ItemType.Equipment);
            this.View.Tier.gameObject.SetActive(this.Model.ItemModel.ItemType == ItemType.Equipment);
        }
    }
}