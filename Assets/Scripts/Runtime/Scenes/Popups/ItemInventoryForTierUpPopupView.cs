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


    public class ItemInventoryForTierUpPopupModel
    {
        public ItemData ItemData { get; }
        public Action<string>   OnSelect { get; }
        public ItemInventoryForTierUpPopupModel(ItemData itemData, Action<string> onSelect)
        {
            this.ItemData = itemData;
            this.OnSelect = onSelect;
        }
    }

    public class ItemInventoryForTierUpPopupView : BaseView
    {
        [field: SerializeField] public ItemInventoryAdapter Adapter              { get; private set; }
        [field: SerializeField] public Button               CloseButton          { get; private set; }
        [field: SerializeField] public RectTransform        CategoryContainer    { get; private set; }
        [field: SerializeField] public RectTransform        ViewField            { get; private set; }
    }

    [PopupInfo(nameof(ItemInventoryForTierUpPopupView), isCloseWhenTapOutside: false, isOverlay: true)]
    public class ItemInventoryForTierUpPopupPresenter : BasePopupPresenter<ItemInventoryForTierUpPopupView, ItemInventoryForTierUpPopupModel>
    {
        private readonly InventoryLocalDataController inventoryLocalDataController;
        private readonly DiContainer                  diContainer;
        private readonly ItemBlueprint                itemBlueprint;
        public ItemInventoryForTierUpPopupPresenter(SignalBus signalBus, ILogService logService, InventoryLocalDataController inventoryLocalDataController,
            DiContainer diContainer, ItemBlueprint itemBlueprint) : base(signalBus, logService)
        {
            this.inventoryLocalDataController = inventoryLocalDataController;
            this.diContainer                  = diContainer;
            this.itemBlueprint                = itemBlueprint;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.CloseButton.onClick.AddListener(this.CloseView);
        }
        public override async UniTask BindData(ItemInventoryForTierUpPopupModel model)
        {
            this.View.ViewField.DOLocalMoveY(- this.ViewHeight, 0);
            this.View.ViewField.DOLocalMoveY(0, 0.5f).SetEase(Ease.InQuad);
            this.BindItems().Forget();
        }
        
        private float ViewHeight      => this.View.ViewField.parent.GetComponent<RectTransform>().rect.height;

        private async UniTaskVoid BindItems()
        {
            var items = this.inventoryLocalDataController.GetItems(ItemType.Equipment)
                .Where(x=>!x.IsEquipped)
                .Where(x=>x.BlueprintId == this.Model.ItemData.BlueprintId)
                .Where(x =>x.InventoryId != this.Model.ItemData.InventoryId)
                .Where(x => x.Tier == this.Model.ItemData.Tier)
                .Where(x=>x.Rarity == this.Model.ItemData.Rarity)
                .Select(x=>x.ToModel(this.itemBlueprint))
                .ToList();
            if (items.Count % this.View.Adapter.Parameters.Grid.MaxCellsPerGroup != 0)
                items.AddRange(Enumerable.Repeat<ItemModel>(new(new() { InventoryId = null }, this.itemBlueprint),
                    this.View.Adapter.Parameters.Grid.MaxCellsPerGroup - items.Count % this.View.Adapter.Parameters.Grid.MaxCellsPerGroup));

            await this.View.Adapter.InitItemAdapter(items.Select(x => new ItemInventoryItemModel(x, null, null, x.Quantity, this.OnSelectItem,null)).ToList(),
                this.diContainer);
        }
        
        private void OnSelectItem(string inventoryId)
        {
            this.Model.OnSelect(inventoryId);
            this.CloseView();
        }
        
        public override void CloseView()
        {
            this.View.ViewField.DOLocalMoveY(- this.ViewHeight, 0.5f).SetEase(Ease.InQuad).onComplete += () => { base.CloseView(); };
        }
    }
}