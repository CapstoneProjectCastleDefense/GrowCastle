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
        public Action CharacterInfoRefresh { get; }
        public ItemInventoryPopupModel(IEquippable equippable, string selectedItemInventoryId, Action characterInfoRefresh)
        {
            this.Equippable              = equippable;
            this.SelectedItemInventoryId = selectedItemInventoryId;
            this.CharacterInfoRefresh    = characterInfoRefresh;
        }
        public IEquippable Equippable              { get; set; }
        public string      SelectedItemInventoryId { get; set; }
    }

    public class ItemInventoryPopupView : BaseView
    {
        [field: SerializeField] public ItemInventoryAdapter Adapter              { get; private set; }
        [field: SerializeField] public Button               CloseButton          { get; private set; }
        [field: SerializeField] public Button               CategoryButtonPrefab { get; private set; }
        [field: SerializeField] public RectTransform        CategoryContainer    { get; private set; }
        [field: SerializeField] public RectTransform        ViewField            { get; private set; }
    }

    [PopupInfo(nameof(ItemInventoryPopupView), isCloseWhenTapOutside: false, isOverlay: true)]
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
            this.BindItems().Forget();
        }

        private async UniTaskVoid BindItems()
        {
            List<ItemModel> items = new();
            if (this.Model.Equippable == null)
            {
                items = this.inventoryLocalDataController.GetAllItems().Select(x => x.ToModel(this.itemBlueprint)).ToList();
            }
            else
            {
                items = this.inventoryLocalDataController.GetItems(ItemType.Equipment).Select(x => x.ToModel(this.itemBlueprint)).ToList();
            }

#if UNITY_EDITOR || CREATIVE
            // if (items.Count(x => x.ItemType == ItemType.Equipment) == 0)
            // {
            //     var enums      = Enum.GetValues(typeof(RarityEnum));
            //     var equipments = this.itemBlueprint.Values.Where(x => x.ItemType == ItemType.Equipment).ToList();
            //     for (var _ = 0; _ < 10; _++)
            //     {
            //         foreach (var itemBlueprintValue in equipments)
            //         {
            //             this.inventoryLocalDataController.AddItem(
            //                 itemBlueprintValue.Id,
            //                 1,
            //                 (RarityEnum)enums.GetValue(Random.Range(0, enums.Length)),
            //                 false,
            //                 1,
            //                 0,
            //                 new()
            //                 {
            //                     { StatEnum.Attack, (typeof(float), Random.Range(1, 10)) },
            //                     { StatEnum.Defense, (typeof(float), Random.Range(1, 10)) },
            //                     { StatEnum.Health, (typeof(float), Random.Range(1, 10)) }
            //                 });
            //         }
            //     }
            //
            //     if (this.Model.Equippable == null)
            //     {
            //         items = this.inventoryLocalDataController.GetAllItems().Select(x => x.ToModel(this.itemBlueprint)).ToList();
            //     }
            //     else
            //     {
            //         items = this.inventoryLocalDataController.GetItems(ItemType.Equipment).Select(x => x.ToModel(this.itemBlueprint)).ToList();
            //     }
            // }
#endif

            if (items.Count % this.View.Adapter.Parameters.Grid.MaxCellsPerGroup != 0)
                items.AddRange(Enumerable.Repeat<ItemModel>(new(new() { InventoryId = null }, this.itemBlueprint),
                    this.View.Adapter.Parameters.Grid.MaxCellsPerGroup - items.Count % this.View.Adapter.Parameters.Grid.MaxCellsPerGroup));

            await this.View.Adapter.InitItemAdapter(items.Select(x => new ItemInventoryItemModel(x, this.Model.Equippable, this.OnRecycle, x.Quantity, null, this.Model.CharacterInfoRefresh)).ToList(),
                this.diContainer);
            if (this.Model.SelectedItemInventoryId.IsNullOrEmpty()) return;
            var index = items.FindIndex(x => x.Id == this.Model.SelectedItemInventoryId);
            this.View.Adapter.SmoothScrollTo(index, 0.5f);
        }

        private void OnRecycle() { this.BindItems().Forget(); }

        public override void CloseView() { this.View.ViewField.DOLocalMoveX(this.ViewWidth, 0.5f).SetEase(Ease.InQuad).onComplete += () => { base.CloseView(); }; }
    }
}