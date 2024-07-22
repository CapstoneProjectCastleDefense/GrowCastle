namespace Runtime.Scenes.Popups
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Scenes.Adapters.DailyReward;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class DailyRewardPopupView : BaseView
    {
        public DailyRewardItemAdapter dailyRewardItemAdapter;
        public Button                 claimBtn;
        public Button                 exitBtn;
        public GameObject             rewardField;
        public Transform              startPos;
        public Transform              endPos;
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
            this.View.rewardField.transform.position = this.View.startPos.position;
            this.View.rewardField.transform.DOMove(this.View.endPos.position, 1f).SetEase(Ease.OutElastic);
            await this.dailyRewardLocalDataController.CheckRewardStatus();
            this.View.claimBtn.interactable = true;
            if (!this.dailyRewardLocalDataController.CheckCanClaim()) this.View.claimBtn.interactable = false;

            var listRewardItemData = this.dailyRewardLocalDataController.GetAllRewardLocalData.Select(data => new DailyRewardItemModel()
                { Day = data.Day }).ToList();
            await this.View.dailyRewardItemAdapter.InitItemAdapter(listRewardItemData, this.diContainer);
        }

        public override void CloseView()
        {
            this.View.rewardField.transform.DOMove(this.View.startPos.position, 1f).SetEase(Ease.InOutQuint).onComplete += () =>
            {
                base.CloseView();
            };
        }
        private void OnClaimReward()
        {
            this.dailyRewardLocalDataController.ClaimAllAvailableReward();
            this.View.dailyRewardItemAdapter.Refresh();
            this.View.claimBtn.interactable = false;
        }
    }
}