namespace Runtime.Scenes.Adapters.ItemInventory
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using global::Extensions;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Scenes.Popups;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ItemInventoryItemModel
    {
        public Action      OnRecycle   { get; }
        public ItemModel   Model       { get; }
        public IEquippable Equippable  { get; }
        public string      InventoryId { get; }
        public ItemInventoryItemModel(ItemModel itemModel, IEquippable equippable, string inventoryId, Action onRecycle)
        {
            this.OnRecycle   = onRecycle;
            this.Model       = itemModel;
            this.Equippable  = equippable;
            this.InventoryId = inventoryId;
        }
    }

    public class ItemInventoryItemView : TViewMono
    {
        public Image    image;
        public Button   button;
        public Image    rarityImage;
        public TMP_Text quantity;
    }

    public class ItemInventoryItemPresenter : BaseUIItemPresenter<ItemInventoryItemView, ItemInventoryItemModel>
    {
        private          ItemInventoryItemModel model;
        private readonly IScreenManager         screenManager;
        public ItemInventoryItemPresenter(IGameAssets gameAssets, IScreenManager screenManager) : base(gameAssets) { this.screenManager = screenManager; }
        public override async void BindData(ItemInventoryItemModel param)
        {
            this.model             = param;
            this.View.button.onClick.RemoveAllListeners();
            if (this.model == null || this.model.InventoryId.IsNullOrEmpty())
            {
                this.View.gameObject.SetActive(false);
                return;
            }
            this.View.gameObject.SetActive(true);
            this.View.image.sprite = await this.GameAssets.LoadAssetAsync<Sprite>(this.model.Model.AddressableName);
            this.View.quantity.gameObject.SetActive(this.model.Model.ItemType != ItemType.Equipment);
            this.View.button.onClick.AddListener(() =>
            {
                this.screenManager
                    .OpenScreen<ItemDetailPopupPresenter, ItemDetailPopupModel>(
                        new(this.model.Model, this.model.Equippable, this.model.InventoryId, this.model.OnRecycle))
                    .Forget();
            });
            this.BindVolatileData();
        }

        private async void BindVolatileData()
        {
            this.View.rarityImage.sprite = await this.GameAssets.LoadAssetAsync<Sprite>(this.model.Model.Rarity.ToString());
            this.View.quantity.text      = this.model.Model.Quantity.ToString();
        }
    }
}