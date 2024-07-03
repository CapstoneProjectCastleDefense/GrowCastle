namespace Runtime.Scenes.Adapters.DailyReward
{
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;

    public class DailyRewardItemModel
    {
        
    }
    public class DailyRewardItemView : TViewMono
    {
        
    }
    
    public class DailyRewardItemPresenter : BaseUIItemPresenter<DailyRewardItemView,DailyRewardItemModel>
    {
        public DailyRewardItemPresenter(IGameAssets gameAssets)
            : base(gameAssets)
        {
        }
        public override void BindData(DailyRewardItemModel param)
        {
            
        }
    }
}