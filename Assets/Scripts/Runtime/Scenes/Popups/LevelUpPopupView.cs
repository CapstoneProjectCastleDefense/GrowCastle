namespace Runtime.Scenes.Popups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Scenes.Adapters.ItemInventory;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class LevelUpPopupModel
    {
        public LevelUpPopupModel(string inventoryId, Action onLevelUp, List<(string, int)> itemsToLevelUp)
        {
            this.OnLevelUp      = onLevelUp;
            this.ItemsToLevelUp = itemsToLevelUp;
            this.InventoryId    = inventoryId;
        }
        public Action              OnLevelUp      { get; private set; }
        public List<(string, int)> ItemsToLevelUp { get; private set; }
        public string              InventoryId    { get; private set; }
    }

    public class LevelUpPopupView : BaseView
    {
        public Button               ConfirmBtn;
        public Button               CancelBtn;
        public ItemInventoryAdapter Adapter;
    }

    [PopupInfo(nameof(LevelUpPopupView), isOverlay: true)]
    public class LevelUpPopupPresenter : BasePopupPresenter<LevelUpPopupView, LevelUpPopupModel>
    {
        private readonly InventoryLocalDataController inventoryLocalDataController;
        private readonly ItemBlueprint                itemBlueprint;
        private readonly DiContainer                  diContainer;
        public LevelUpPopupPresenter(SignalBus signalBus, ILogService logService, InventoryLocalDataController inventoryLocalDataController, ItemBlueprint itemBlueprint,
            DiContainer diContainer)
            : base(signalBus, logService)
        {
            this.inventoryLocalDataController = inventoryLocalDataController;
            this.itemBlueprint                = itemBlueprint;
            this.diContainer                  = diContainer;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.CancelBtn.onClick.AddListener(this.CloseView);
            this.View.ConfirmBtn.onClick.AddListener(() =>
            {
                this.inventoryLocalDataController.GetItem(this.Model.InventoryId).Level++;
                var localData = this.inventoryLocalDataController.GetItems(ItemType.InventoryResource);
                foreach (var item in this.Model.ItemsToLevelUp)
                {
                    localData.FirstOrDefault(x => x.BlueprintId == item.Item1).Quantity -= item.Item2;
                }

                this.Model.OnLevelUp?.Invoke();
                this.CloseView();
            });
        }
        public override async UniTask BindData(LevelUpPopupModel popupModel)
        {
            var list       = new List<ItemInventoryItemModel>();
            var localData  = this.inventoryLocalDataController.GetItems(ItemType.InventoryResource);
            var canLevelUp = true;
            foreach (var item in popupModel.ItemsToLevelUp)
            {
                var itemData = localData.FirstOrDefault(x => x.BlueprintId == item.Item1);
                if (itemData == null)
                {
                    Debug.LogError("Item not found");
                    continue;
                }

                list.Add(new(itemData.ToModel(this.itemBlueprint), null, null, item.Item2, null, null));
                if (itemData.Quantity < item.Item2)
                {
                    canLevelUp = false;
                }
            }

            this.View.ConfirmBtn.interactable = canLevelUp;
            await this.View.Adapter.InitItemAdapter(list, this.diContainer);
        }
    }
}