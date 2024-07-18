namespace Runtime.Scenes.Popups
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Scenes.Adapters.ItemInventory;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;


    public class ItemInventoryPopupModel
    {
        public ItemInventoryPopupModel(IEquippable equippable) { this.Equippable = equippable; }
        public IEquippable Equippable { get; set; }
    }

    public class ItemInventoryPopupView : BaseView
    {
        [field: SerializeField] public ItemInventoryAdapter Adapter              { get; private set; }
        [field: SerializeField] public Button               CloseButton          { get; private set; }
        [field: SerializeField] public Button               CategoryButtonPrefab { get; private set; }
        [field: SerializeField] public Transform            CategoryContainer    { get; private set; }
    }

    [PopupInfo(nameof(ItemInventoryPopupView), isCloseWhenTapOutside: false)]
    public class ItemInventoryPopupPresenter : BasePopupPresenter<ItemInventoryPopupView, ItemInventoryPopupModel>
    {
        private readonly InventoryLocalDataController inventoryLocalDataController;
        private readonly DiContainer                  diContainer;
        public ItemInventoryPopupPresenter(SignalBus signalBus, ILogService logService, InventoryLocalDataController inventoryLocalDataController,
            DiContainer diContainer) : base(signalBus, logService)
        {
            this.inventoryLocalDataController = inventoryLocalDataController;
            this.diContainer                  = diContainer;
        }
        public override async UniTask BindData(ItemInventoryPopupModel model)
        {
            List<IItemModel> items = new();
            if (model.Equippable == null)
            {
                items = this.inventoryLocalDataController.GetAllItems();
            }
            else
            {
                items = this.inventoryLocalDataController.GetItems(ItemType.Equipment);
            }

            await this.View.Adapter.InitItemAdapter(items.Select(x => new ItemInventoryItemModel(x, this.Model.Equippable)).ToList(), this.diContainer);
        }
    }
}