namespace Runtime.Scenes.Adapters.Chest
{
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using Models.Blueprints;
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
        private readonly ResourceBlueprint resourceBlueprint;
        public ChestRewardItemPresenter(IGameAssets gameAssets, ResourceBlueprint resourceBlueprint)
            : base(gameAssets)
        {
            this.resourceBlueprint = resourceBlueprint;
        }
        public override void BindData(ChestRewardItemModel param)
        {
            string iconImage = "";
            if (this.resourceBlueprint.ContainsKey(param.PoolItem.ItemType))
            {
                iconImage = this.resourceBlueprint.GetDataById(param.PoolItem.ItemType).Image;
            }
            this.View.icon.sprite = this.GameAssets.LoadAssetAsync<Sprite>(iconImage).WaitForCompletion();
            this.View.value.text  = $"{param.PoolItem.Value}";
        }
    }
}