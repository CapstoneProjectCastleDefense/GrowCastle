namespace Runtime.Scenes.Popups
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Services;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using TMPro;
    using UnityEngine.UI;
    using Zenject;

    public class DungeonConfirmPopupModel
    {
        public Action onConfirmAction;
        public string dungeonId;
    }

    public class DungeonConfirmPopupView : BaseView
    {
        public TextMeshProUGUI ticketValue;
        public Button          cancelBtn;
        public Button          confirmBtn;
    }

    [PopupInfo(nameof(DungeonConfirmPopupView), isOverlay: true)]
    public class DungeonConfirmPopupPresenter : BasePopupPresenter<DungeonConfirmPopupView, DungeonConfirmPopupModel>
    {
        private readonly DungeonLocalDataController  dungeonLocalDataController;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly GameStateMachine            gameStateMachine;
        private readonly ToastController             toastController;

        public DungeonConfirmPopupPresenter(SignalBus signalBus, ILogService logService, DungeonLocalDataController dungeonLocalDataController, ResourceLocalDataController resourceLocalDataController, GameStateMachine gameStateMachine, ToastController toastController) : base(signalBus, logService)
        {
            this.dungeonLocalDataController  = dungeonLocalDataController;
            this.resourceLocalDataController = resourceLocalDataController;
            this.gameStateMachine            = gameStateMachine;
            this.toastController             = toastController;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.confirmBtn.onClick.AddListener(this.OnConfirmBtnClick);
            this.View.cancelBtn.onClick.AddListener(this.CloseView);
        }

        public override async UniTask BindData(DungeonConfirmPopupModel popupModel)
        {
            this.Model                 = popupModel;
            this.View.ticketValue.text = $"{this.dungeonLocalDataController.GetDungeonRecord(popupModel.dungeonId).Ticket}";
        }

        private void OnConfirmBtnClick()
        {
            this.EnterDungeon(this.Model.dungeonId);
        }

        private void EnterDungeon(string dungeonId)
        {
            var dungeonRecord = this.dungeonLocalDataController.GetDungeonRecord(dungeonId);
            if (this.resourceLocalDataController.SpendResource(ResourceType.Ticket, dungeonRecord.Ticket))
            {
                this.dungeonLocalDataController.currentSelectedDungeon = dungeonId;
                this.gameStateMachine.TransitionTo<GameDungeonModeState>();
                this.Model.onConfirmAction?.Invoke();

                this.CloseView();
            }
            else
            {
                this.toastController.ShowToast("Not enough ticket");
            }
        }
    }
}