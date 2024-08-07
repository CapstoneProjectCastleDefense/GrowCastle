namespace Runtime.Scenes.Popups
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Services;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using TMPro;
    using UnityEngine.UI;
    using Zenject;
    using R3;

    public class DungeonSelectLevelPopupView : BaseView
    {
        public Button                 exitButton;
        public List<DungeonLevelItem> dungeonLevelItems;
        public TextMeshProUGUI        ticketValue;
    }

    [PopupInfo(nameof(DungeonSelectLevelPopupView), isOverlay: true)]
    public class DungeonSelectLevelPopupPresenter : BasePopupPresenter<DungeonSelectLevelPopupView>
    {
        private readonly DungeonLocalDataController  dungeonLocalDataController;
        private readonly GameStateMachine            gameStateMachine;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly ToastController             toastController;

        public DungeonSelectLevelPopupPresenter(SignalBus signalBus, DungeonLocalDataController dungeonLocalDataController, GameStateMachine gameStateMachine, ResourceLocalDataController resourceLocalDataController, ToastController toastController) : base(signalBus)
        {
            this.dungeonLocalDataController  = dungeonLocalDataController;
            this.gameStateMachine            = gameStateMachine;
            this.resourceLocalDataController = resourceLocalDataController;
            this.toastController             = toastController;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.exitButton.onClick.AddListener(this.CloseView);
            this.resourceLocalDataController.GetResource(ResourceType.Ticket).Subscribe(this.OnTickResourceValueChange);
        }

        public override UniTask BindData()
        {
            this.dungeonLocalDataController.CheckStatusOfAllDungeon();
            this.View.dungeonLevelItems.ForEach(item =>
            {
                item.dungeonIdText.text      =  item.dungeonId;
                item.onDungeonSelectBtnClick =  null;
                item.onDungeonSelectBtnClick += this.EnterDungeon;
                item.gameObject.SetActive(this.dungeonLocalDataController.CheckDungeonIsUnlock(item.dungeonId));
            });
            this.View.ticketValue.text = $"{this.resourceLocalDataController.GetResource(ResourceType.Ticket).Value}";

            return UniTask.CompletedTask;
        }

        private void OnTickResourceValueChange(float value)
        {
            if (this.View == null) return;
            this.View.ticketValue.text = $"{value}";
        }

        private void EnterDungeon(string dungeonId)
        {
            var dungeonRecord = this.dungeonLocalDataController.GetDungeonRecord(dungeonId);
            if (this.resourceLocalDataController.SpendResource(ResourceType.Ticket, dungeonRecord.Ticket))
            {
                this.dungeonLocalDataController.currentSelectedDungeon = dungeonId;
                this.gameStateMachine.TransitionTo<GameDungeonModeState>();
                this.CloseView();
            }
            else
            {
                this.toastController.ShowToast("Not enough ticket");
            }
        }
    }
}