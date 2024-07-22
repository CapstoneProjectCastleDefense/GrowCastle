namespace Runtime.Scenes.Popups
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Scenes.Adapters.Quests;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class QuestPopupView : BaseView
    {
        public Button       dailyQuestButton;
        public Button       weeklyQuestButton;
        public Button       achievementQuestButton;
        public Button       exitButton;
        public QuestAdapter questAdapter;
        
        public Transform  startPos;
        public Transform  endPos;
        public GameObject viewField;
    }

    [PopupInfo(nameof(QuestPopupView), isOverlay: true)]
    public class QuestPopupPresenter : BasePopupPresenter<QuestPopupView>
    {
        private readonly QuestLocalDataController questLocalDataController;
        private readonly DiContainer              diContainer;
        public QuestPopupPresenter(SignalBus signalBus, QuestLocalDataController questLocalDataController, DiContainer diContainer)
            : base(signalBus)
        {
            this.questLocalDataController = questLocalDataController;
            this.diContainer              = diContainer;
        }
        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.dailyQuestButton.onClick.AddListener(this.InitDailyQuest);
            this.View.weeklyQuestButton.onClick.AddListener(this.InitWeeklyQuest);
            this.View.achievementQuestButton.onClick.AddListener(this.InitAchievementQuest);
            this.View.exitButton.onClick.AddListener(this.CloseView);
            this.InitDailyQuest();
        }
        public override UniTask BindData()
        {
            this.View.viewField.transform.position = this.View.startPos.position;
            this.View.viewField.transform.DOMove(this.View.endPos.position, 0.5f).SetEase(Ease.InOutQuint);
            return UniTask.CompletedTask;
        }

        private void InitDailyQuest() { this.InitQuestWithType(QuestType.Daily); }

        private void InitWeeklyQuest() { this.InitQuestWithType(QuestType.Weekly); }

        private void InitAchievementQuest() { this.InitQuestWithType(QuestType.Achievement); }

        private async void InitQuestWithType(QuestType questType)
        {
            var listData = this.questLocalDataController.GetAllQuestWithType(questType).Select(e => new QuestItemModel() { QuestId = e.QuestId }).ToList();
            await this.View.questAdapter.InitItemAdapter(listData, this.diContainer);
        }
        public override void CloseView()
        {
            this.View.viewField.transform.DOMove(this.View.startPos.position, 0.5f).SetEase(Ease.InOutQuint).onComplete += () =>
            {
                base.CloseView();
            };
        }
    }
}