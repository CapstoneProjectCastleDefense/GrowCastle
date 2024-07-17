namespace Runtime.Scenes.Adapters.ItemInventory
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using Runtime.Interfaces.Items;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ItemInventoryItemModel
    {
        public IItemModel ItemModel;
        public ItemInventoryItemModel(IItemModel itemModel) { this.ItemModel = itemModel; }
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
        public ItemInventoryItemPresenter(IGameAssets gameAssets) : base(gameAssets) { }
        public override async void BindData(ItemInventoryItemModel param)
        {
            this.View.image.sprite       = await this.GameAssets.LoadAssetAsync<Sprite>("");
            this.View.rarityImage.sprite = await this.GameAssets.LoadAssetAsync<Sprite>("");
            this.View.quantity.text      = param.ItemModel.Quantity.ToString();
            this.View.button.onClick.AddListener(() =>
            {
                
            });
        }
    }
}