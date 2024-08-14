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
    using TMPro;
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
        [field: SerializeField] public ItemInventoryAdapter Adapter               { get; private set; }
        [field: SerializeField] public Button               CloseButton           { get; private set; }
        [field: SerializeField] public RectTransform        ViewField             { get; private set; }
        [field: SerializeField] public List<Button>         CategoryButtons       { get; private set; }
        [field: SerializeField] public List<Button>         CategoryButtonsHolder { get; private set; }
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
        private float ViewHeight => this.View.ViewField.parent.GetComponent<RectTransform>().rect.height;

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.CloseButton.onClick.AddListener(this.CloseView);
            this.View.CategoryButtons.ForEach(x => x.onClick.AddListener(() => this.OnCategorySelected(x.name)));
        }
        private void OnCategorySelected(string objName)
        {
            foreach (var button in this.View.CategoryButtonsHolder)
            {
                button.targetGraphic.enabled = button.name == objName;
                button.GetComponentInChildren<TMP_Text>(includeInactive:true).gameObject.SetActive(button.name == objName);
            }

            switch (objName)
            {
                case "CategoryButtonAll":
                    this.BindItems().Forget();
                    break;
                case "CategoryButtonC":
                    this.BindItems(RarityEnum.Common).Forget();
                    break;
                case "CategoryButtonR":
                    this.BindItems(RarityEnum.Rare).Forget();
                    break;
                case "CategoryButtonL":
                    this.BindItems(RarityEnum.Legendary).Forget();
                    break;
            }
        }
        public override async UniTask BindData(ItemInventoryPopupModel model)
        {
            this.View.ViewField.DOLocalMoveY(-this.ViewHeight, 0);
            this.View.ViewField.DOLocalMoveY(0, 0.5f).SetEase(Ease.InQuad);
            this.OnCategorySelected("CategoryButtonAll");
        }

        private async UniTaskVoid BindItems(RarityEnum rarityEnum = RarityEnum.None)
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

            if (rarityEnum != RarityEnum.None)
            {
                items = items.Where(x => x.Rarity == rarityEnum).ToList();
            }

            if (items.Count % this.View.Adapter.Parameters.Grid.MaxCellsPerGroup != 0)
                items.AddRange(Enumerable.Repeat<ItemModel>(new(new() { InventoryId = null }, this.itemBlueprint),
                    this.View.Adapter.Parameters.Grid.MaxCellsPerGroup - items.Count % this.View.Adapter.Parameters.Grid.MaxCellsPerGroup));
            var models = items.Select(x => new ItemInventoryItemModel(x, this.Model.Equippable, this.OnRecycle, x.Quantity, null, this.Model.CharacterInfoRefresh))
                .ToList();
            await this.View.Adapter.InitItemAdapter(models, this.diContainer);
        }

        private void OnRecycle() { this.BindItems().Forget(); }

        public override void CloseView() { this.View.ViewField.DOLocalMoveY(-this.ViewHeight, 0.5f).SetEase(Ease.InQuad).onComplete += () => { base.CloseView(); }; }
    }
}