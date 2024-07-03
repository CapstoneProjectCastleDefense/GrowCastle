namespace Runtime.Scenes.Popups
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Runtime.Scenes.Adapters.DailyReward;
    using UnityEngine.UI;
    using Zenject;

    public class DailyRewardPopupView : BaseView
    {
        public DailyRewardItemAdapter dailyRewardItemAdapter;
        public Button                 claimBtn;
        public Button                 exitBtn;
    }
    [PopupInfo(nameof(DailyRewardPopupView), isOverlay: true)]
    public class DailyRewardPopupPresenter : BasePopupPresenter<DailyRewardPopupView>
    {
        public DailyRewardPopupPresenter(SignalBus signalBus)
            : base(signalBus)
        {
        }
        public override UniTask BindData()
        {
            return UniTask.CompletedTask;
        }
    }
}