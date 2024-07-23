namespace Runtime.Scenes.Popups
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using global::Extensions;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Equipment;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Scenes.Adapters.ItemInventory;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;


    public class ItemInventoryPopupModel
    {
        public ItemInventoryPopupModel(IEquippable equippable, string id)
        {
            this.Equippable = equippable;
            this.Id         = id;
        }
        public IEquippable Equippable { get; set; }
        public string      Id         { get; set; }
    }

    public class ItemInventoryPopupView : BaseView
    {
        [field: SerializeField] public ItemInventoryAdapter Adapter              { get; private set; }
        [field: SerializeField] public Button               CloseButton          { get; private set; }
        [field: SerializeField] public Button               CategoryButtonPrefab { get; private set; }
        [field: SerializeField] public RectTransform        CategoryContainer    { get; private set; }
        [field: SerializeField] public RectTransform        ViewField            { get; private set; }
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

        private float ViewFieldWidth => this.View.ViewField.rect.width + this.View.CategoryContainer.rect.width;
        private float ViewWidth      => this.View.ViewField.parent.GetComponent<RectTransform>().rect.width;

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.CloseButton.onClick.AddListener(this.CloseView);
        }
        public override async UniTask BindData(ItemInventoryPopupModel model)
        {
            this.View.ViewField.DOLocalMoveX(this.ViewWidth, 0);
            this.View.ViewField.DOLocalMoveX(this.ViewWidth - this.ViewFieldWidth * 1.5f, 0.5f).SetEase(Ease.InQuad);
            Dictionary<string,IItemModel> items = new();
            if (model.Equippable == null)
            {
                items = this.inventoryLocalDataController.GetAllItems();
            }
            else
            {
                items = this.inventoryLocalDataController.GetItems(ItemType.Equipment);
            }

            if (items.Count == 0)
            {
                this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "Test", new()
                {
                    { StatEnum.Attack, (typeof(int), 10) },
                    { StatEnum.Defense, (typeof(int), 10) },
                    { StatEnum.Health, (typeof(int), 10) }
                }, EquipmentType.Weapon, ItemType.Equipment, RarityEnum.Common, 1));
                await UniTask.Delay(100);
                this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "Test", new(), EquipmentType.Weapon, ItemType.Equipment,
                    RarityEnum.Legendary, 1));
                await UniTask.Delay(100);
                this.inventoryLocalDataController.AddItem(
                    new EquipmentModel("Environment_1", "Test", new(), EquipmentType.Weapon, ItemType.Equipment, RarityEnum.Rare, 1));
                await UniTask.Delay(100);
                this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "Test", new(), EquipmentType.Weapon, ItemType.Equipment, RarityEnum.Common,
                    1));
                await UniTask.Delay(100);
                this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "Test", new(), EquipmentType.Weapon, ItemType.Equipment,
                    RarityEnum.Legendary, 1));
            }

            if (model.Equippable == null)
            {
                items = this.inventoryLocalDataController.GetAllItems();
            }
            else
            {
                items = this.inventoryLocalDataController.GetItems(ItemType.Equipment);
            }

            await this.View.Adapter.InitItemAdapter(items.Select(x => new ItemInventoryItemModel(x.Value, this.Model.Equippable, x.Key)).ToList(), this.diContainer);
            if (this.Model.Id.IsNullOrEmpty()) return;
            var index = items.Values.ToList().FindIndex(x => x.Id == this.Model.Id);
            this.View.Adapter.SmoothScrollTo(index, 0.5f);
        }

        public override void CloseView() { this.View.ViewField.DOLocalMoveX(this.ViewWidth, 0.5f).SetEase(Ease.InQuad).onComplete += () => { base.CloseView(); }; }
    }
}