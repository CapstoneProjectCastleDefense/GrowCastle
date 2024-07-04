namespace Runtime.Scenes.Popups
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
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
        private readonly DailyRewardLocalDataController dailyRewardLocalDataController;
        private readonly DailyRewardBlueprint           dailyRewardBlueprint;
        private readonly DiContainer                    diContainer;
        public DailyRewardPopupPresenter(SignalBus signalBus, DailyRewardLocalDataController dailyRewardLocalDataController, DailyRewardBlueprint dailyRewardBlueprint, DiContainer diContainer)
            : base(signalBus)
        {
            this.dailyRewardLocalDataController = dailyRewardLocalDataController;
            this.dailyRewardBlueprint           = dailyRewardBlueprint;
            this.diContainer                    = diContainer;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.exitBtn.onClick.AddListener(this.CloseView);
            this.View.claimBtn.onClick.AddListener(this.OnClaimReward);
        }
        public override async UniTask BindData()
        {
            await this.dailyRewardLocalDataController.CheckRewardStatus();
            this.View.claimBtn.interactable = true;
            if (!this.dailyRewardLocalDataController.CheckCanClaim()) this.View.claimBtn.interactable = false;

            var listRewardItemData = this.dailyRewardLocalDataController.GetAllRewardLocalData.Select(data => new DailyRewardItemModel()
                { Day = data.Day }).ToList();
            await this.View.dailyRewardItemAdapter.InitItemAdapter(listRewardItemData, this.diContainer);
        }

        private void OnClaimReward()
        {
            this.dailyRewardLocalDataController.ClaimAllAvailableReward();
            this.View.dailyRewardItemAdapter.Refresh();
            this.View.claimBtn.interactable = false;
        }
    }
}