namespace Runtime.Scenes.Adapters.ItemInventory
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Scenes.Popups;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ItemInventoryItemModel
    {
        public readonly ItemModel  ItemModel;
        public readonly IEquippable Equippable;
        public readonly string      InventoryId;
        public ItemInventoryItemModel(ItemModel itemModel, IEquippable equippable, string inventoryId)
        {
            this.ItemModel   = itemModel;
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
        private readonly IScreenManager screenManager;
        public ItemInventoryItemPresenter(IGameAssets gameAssets, IScreenManager screenManager) : base(gameAssets) { this.screenManager = screenManager; }
        public override async void BindData(ItemInventoryItemModel param)
        {
            this.View.image.sprite       = await this.GameAssets.LoadAssetAsync<Sprite>(param.ItemModel.AddressableName);
            this.View.rarityImage.sprite = await this.GameAssets.LoadAssetAsync<Sprite>(param.ItemModel.Rarity.ToString());
            this.View.quantity.text      = param.ItemModel.Quantity.ToString();
            this.View.button.onClick.AddListener(() =>
            {
                this.screenManager.OpenScreen<ItemDetailPopupPresenter, ItemDetailPopupModel>(new(param.ItemModel, param.Equippable, param.InventoryId)).Forget();
            });
        }
    }
}