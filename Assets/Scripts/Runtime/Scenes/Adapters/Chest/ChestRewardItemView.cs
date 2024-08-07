namespace Runtime.Scenes.Adapters.Chest
{
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Extensions;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class ChestRewardItemModel
    {
        public PoolItem PoolItem;
    }
    public class ChestRewardItemView : TViewMono
    {
        public Image           icon;
        public TextMeshProUGUI value;
    }

    public class ChestRewardItemPresenter : BaseUIItemPresenter<ChestRewardItemView,ChestRewardItemModel>
    {
        private readonly ResourceBlueprint            resourceBlueprint;
        private readonly InventoryLocalDataController inventoryLocalDataController;
        private readonly ItemBlueprint                itemBlueprint;
        public ChestRewardItemPresenter(IGameAssets gameAssets, ResourceBlueprint resourceBlueprint, InventoryLocalDataController inventoryLocalDataController, ItemBlueprint itemBlueprint)
            : base(gameAssets)
        {
            this.resourceBlueprint            = resourceBlueprint;
            this.inventoryLocalDataController = inventoryLocalDataController;
            this.itemBlueprint                = itemBlueprint;
        }
        public override void BindData(ChestRewardItemModel param)
        {
            var iconImage = "";
            if (param.PoolItem.ItemId.IsStringInEnum<ResourceType>())
            {
                iconImage = this.resourceBlueprint.GetDataById(param.PoolItem.ItemId.ToEnum<ResourceType>()).Image;
            }
            else
            {
                var itemId = param.PoolItem.ItemId.Split("|")[0];
                var item   = this.inventoryLocalDataController.GetItem(itemId);
                iconImage = this.itemBlueprint.GetDataById(itemId).ImageAddress;
            }
            this.View.icon.sprite = this.GameAssets.LoadAssetAsync<Sprite>(iconImage).WaitForCompletion();
            this.View.value.text  = $"{param.PoolItem.Value}";
        }
    }
}