namespace Runtime.Scenes.Popups
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.StaticValues;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class ItemDetailPopupModel
    {
        public ItemModel   ItemModel            { get; }
        public IEquippable Equippable           { get; }
        public string      InventoryId          { get; }
        public Action      OnRecycle            { get; }
        public Action      CharacterInfoRefresh { get; }
        public ItemDetailPopupModel(ItemModel itemModel, IEquippable equippable, string inventoryId, Action onRecycle, Action characterInfoRefresh)
        {
            this.ItemModel            = itemModel;
            this.Equippable           = equippable;
            this.InventoryId          = inventoryId;
            this.OnRecycle            = onRecycle;
            this.CharacterInfoRefresh = characterInfoRefresh;
        }
    }

    public class ItemDetailPopupView : BaseView
    {
        public Button   CloseButton;
        public Button   EquipButton;
        public Button   UnequipButton;
        public Button   LevelButton;
        public Button   TierButton;
        public Button   RecycleButton;
        public Image    ItemImage;
        public Image    RarityImage;
        public TMP_Text Description;
        public TMP_Text Quantity;
        public TMP_Text Level;
        public TMP_Text Tier;
        public TMP_Text Name;
    }

    [PopupInfo(nameof(ItemDetailPopupView), isCloseWhenTapOutside: false, isOverlay: true)]
    public class ItemDetailPopupPresenter : BasePopupPresenter<ItemDetailPopupView, ItemDetailPopupModel>
    {
        private readonly IGameAssets                  gameAssets;
        private readonly IScreenManager               screenManager;
        private readonly InventoryLocalDataController inventoryLocalDataController;
        public ItemDetailPopupPresenter(SignalBus signalBus, ILogService logService, IGameAssets gameAssets, IScreenManager screenManager,
            InventoryLocalDataController inventoryLocalDataController) : base(signalBus, logService)
        {
            this.gameAssets                   = gameAssets;
            this.screenManager                = screenManager;
            this.inventoryLocalDataController = inventoryLocalDataController;
        }

        private IScreenPresenter screenPresenter;

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.EquipButton.onClick.AddListener(this.OnEquip);
            this.View.UnequipButton.onClick.AddListener(this.OnUnEquip);
            this.View.CloseButton.onClick.AddListener(this.CloseView);

            this.View.LevelButton.onClick.AddListener(async () =>
            {
                this.screenPresenter = await this.screenManager.OpenScreen<LevelUpPopupPresenter, LevelUpPopupModel>(new(this.Model.InventoryId, this.OnLevelUp,
                    new()
                    {
                        (MiscValue.ItemFragment, this.Model.ItemModel.Level),
                    })
                );
            });

            this.View.TierButton.onClick.AddListener(async () =>
            {
                this.screenPresenter = await this.screenManager.OpenScreen<TierUpPopupPresenter, TierUpPopupModel>(new(this.Model.InventoryId, this.OnTierUp));
            });

            this.View.RecycleButton.onClick.AddListener(() =>
            {
                this.inventoryLocalDataController.RecycleItem(this.Model.InventoryId);
                this.Model.OnRecycle();
                this.CloseView();
            });
        }
        private void OnUnEquip()
        {
            this.Model.Equippable.UnEquip(this.Model.InventoryId);
            this.inventoryLocalDataController.UnEquipItem(this.Model.InventoryId);
            this.BindVolatileData();
            this.Model.CharacterInfoRefresh?.Invoke();
        }
        private void OnEquip()
        {
            this.Model.Equippable.Equip(this.Model.InventoryId);
            this.inventoryLocalDataController.EquipItem(this.Model.InventoryId);
            this.BindVolatileData();
            this.Model.CharacterInfoRefresh?.Invoke();
        }

        private void OnLevelUp()
        {
            this.Model.OnRecycle();
            this.screenPresenter = null;
            this.BindVolatileData();
        }

        private void OnTierUp()
        {
            this.Model.OnRecycle();
            this.screenPresenter = null;
            this.BindVolatileData();
        }

        public override async UniTask BindData(ItemDetailPopupModel popupModel)
        {
            this.Model                   = popupModel;
            this.View.RarityImage.sprite = await this.gameAssets.LoadAssetAsync<Sprite>(this.Model.ItemModel.Rarity.ToString());
            this.View.ItemImage.sprite   = await this.gameAssets.LoadAssetAsync<Sprite>(this.Model.ItemModel.AddressableName);
            this.View.Name.text          = this.Model.ItemModel.Name;
            this.View.Level.gameObject.SetActive(this.Model.ItemModel.ItemType == ItemType.Equipment);
            this.View.Tier.gameObject.SetActive(this.Model.ItemModel.ItemType == ItemType.Equipment);
            this.View.Quantity.gameObject.SetActive(this.Model.ItemModel.ItemType != ItemType.Equipment);
            this.BindVolatileData();
        }

        private void BindVolatileData()
        {
            this.View.Quantity.text = this.Model.ItemModel.Quantity.ToString();
            this.View.Description.text = this.Model.ItemModel.BaseStats is { Count: > 0 }
                ? this.Model.ItemModel.BaseStats
                    .Select(stat => $"{stat.Key}: {this.Model.ItemModel.GetFinalStat(stat.Key, out _):N1}")
                    .Aggregate((current, next) => $"{current}\n{next}")
                : "";
            var canTierUp = this.Model.ItemModel.Level == (this.Model.ItemModel.Tier + 1) * 10;
            this.View.Level.text = $"Level: {this.Model.ItemModel.Level}";
            this.View.Tier.text  = $"Tier: {this.Model.ItemModel.Tier}";
            this.View.EquipButton.gameObject.SetActive(
                this.Model.ItemModel.ItemType == ItemType.Equipment
                && this.Model.Equippable != null
                && !this.Model.ItemModel.IsEquipped
                && this.Model.Equippable.CanEquip());
            this.View.UnequipButton.gameObject.SetActive(
                this.Model.ItemModel.ItemType == ItemType.Equipment
                && this.Model.Equippable != null
                && this.Model.ItemModel.IsEquipped);
            this.View.LevelButton.gameObject.SetActive(this.Model.ItemModel.ItemType == ItemType.Equipment && !canTierUp);
            this.View.TierButton.gameObject.SetActive(this.Model.ItemModel.ItemType == ItemType.Equipment && canTierUp);
            this.View.RecycleButton.gameObject.SetActive(this.Model.ItemModel.ItemType == ItemType.Equipment && !this.Model.ItemModel.IsEquipped);
        }

        public override void CloseView()
        {
            base.CloseView();
            this.screenPresenter?.CloseView();
        }
    }
}