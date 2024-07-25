namespace Runtime.Scenes.Popups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using global::Extensions;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Scenes.Adapters.ItemInventory;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;
    using Random = UnityEngine.Random;


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
        private readonly ItemBlueprint                itemBlueprint;
        public ItemInventoryPopupPresenter(SignalBus signalBus, ILogService logService, InventoryLocalDataController inventoryLocalDataController,
            DiContainer diContainer, ItemBlueprint itemBlueprint) : base(signalBus, logService)
        {
            this.inventoryLocalDataController = inventoryLocalDataController;
            this.diContainer                  = diContainer;
            this.itemBlueprint                = itemBlueprint;
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
            List<ItemModel> items = new();
            if (model.Equippable == null)
            {
                items = this.inventoryLocalDataController.GetAllItems().Select(x => x.ToModel(this.itemBlueprint)).ToList();
            }
            else
            {
                items = this.inventoryLocalDataController.GetItems(ItemType.Equipment).Select(x => x.ToModel(this.itemBlueprint)).ToList();
            }

            if (items.Count == 0)
            {
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "dagger", new()
                // {
                //     { StatEnum.Attack, (typeof(int), 10) },
                //     { StatEnum.Defense, (typeof(int), 10) },
                //     { StatEnum.Health, (typeof(int), 10) }
                // }, EquipmentType.Weapon, ItemType.Equipment, RarityEnum.Common, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "axe", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Legendary, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(
                //     new EquipmentModel("Environment_1", "book", new(), EquipmentType.Weapon, ItemType.Equipment, RarityEnum.Rare, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "owlstaff", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Common,
                //     1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "book", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Legendary, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "bow", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Legendary, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "spear of heaven", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Common, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "snow talimans", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Common, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "sword of kunasagi", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Legendary, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "Locket of Harmony", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Legendary, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "Staff of the Damned", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Rare, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "staff", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Legendary, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "bua cu", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Common, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "frostbite", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Rare, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "amulet", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Rare, 1, 0, 0));
                // await UniTask.Delay(100);
                // this.inventoryLocalDataController.AddItem(new EquipmentModel("Environment_1", "ring", new(), EquipmentType.Weapon, ItemType.Equipment,
                //     RarityEnum.Common, 1, 0, 0));

                var enums = Enum.GetValues(typeof(RarityEnum));
                foreach (var itemBlueprintValue in this.itemBlueprint.Values)
                {
                    this.inventoryLocalDataController.AddItem(
                        new ItemModel(
                            itemBlueprintValue.Id,
                            new()
                            {
                                { StatEnum.Attack, (typeof(float), Random.Range(1, 10)) },
                                { StatEnum.Defense, (typeof(float), Random.Range(1, 10)) },
                                { StatEnum.Health, (typeof(float), Random.Range(1, 10)) }
                            },
                            itemBlueprintValue.ItemType,
                            1,
                            (RarityEnum)enums.GetValue(Random.Range(0, enums.Length)),
                            itemBlueprintValue.ImageAddress,
                            false,
                            1,
                            0,
                            EquipmentType.Weapon));
                }

                if (model.Equippable == null)
                {
                    items = this.inventoryLocalDataController.GetAllItems().Select(x => x.ToModel(this.itemBlueprint)).ToList();
                }
                else
                {
                    items = this.inventoryLocalDataController.GetItems(ItemType.Equipment).Select(x => x.ToModel(this.itemBlueprint)).ToList();
                }
            }


            await this.View.Adapter.InitItemAdapter(items.Select(x => new ItemInventoryItemModel(x, this.Model.Equippable, x.Id)).ToList(), this.diContainer);
            if (this.Model.Id.IsNullOrEmpty()) return;
            var index = items.FindIndex(x => x.Id == this.Model.Id);
            this.View.Adapter.SmoothScrollTo(index, 0.5f);
        }

        public override void CloseView() { this.View.ViewField.DOLocalMoveX(this.ViewWidth, 0.5f).SetEase(Ease.InQuad).onComplete += () => { base.CloseView(); }; }
    }
}