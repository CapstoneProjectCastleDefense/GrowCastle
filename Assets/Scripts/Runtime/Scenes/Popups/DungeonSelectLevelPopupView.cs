namespace Runtime.Scenes.Popups
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Services;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using TMPro;
    using UnityEngine.UI;
    using Zenject;
    using R3;
    using UnityEngine;

    public class DungeonSelectLevelPopupView : BaseView
    {
        public Button                 exitButton;
        public List<DungeonLevelItem> dungeonLevelItems;
        public TextMeshProUGUI        ticketValue;
        public Transform              startPos;
        public Transform              endPos;
        public GameObject             viewField;
    }

    [PopupInfo(nameof(DungeonSelectLevelPopupView), isOverlay: true)]
    public class DungeonSelectLevelPopupPresenter : BasePopupPresenter<DungeonSelectLevelPopupView>
    {
        private readonly DungeonLocalDataController  dungeonLocalDataController;
        private readonly GameStateMachine            gameStateMachine;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly ToastController             toastController;
        private readonly LevelLocalDataController    levelLocalDataController;
        private readonly ScreenManager               screenManager;

        public DungeonSelectLevelPopupPresenter(SignalBus signalBus, DungeonLocalDataController dungeonLocalDataController, GameStateMachine gameStateMachine, ResourceLocalDataController resourceLocalDataController, ToastController toastController, LevelLocalDataController levelLocalDataController, ScreenManager screenManager) : base(signalBus)
        {
            this.dungeonLocalDataController  = dungeonLocalDataController;
            this.gameStateMachine            = gameStateMachine;
            this.resourceLocalDataController = resourceLocalDataController;
            this.toastController             = toastController;
            this.levelLocalDataController    = levelLocalDataController;
            this.screenManager               = screenManager;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.exitButton.onClick.AddListener(this.CloseView);
            this.resourceLocalDataController.GetResource(ResourceType.Ticket).Subscribe(this.OnTickResourceValueChange);
        }

        public override UniTask BindData()
        {
            this.View.viewField.transform.position = this.View.startPos.position;
            this.View.viewField.transform.DOMove(this.View.endPos.position, 0.5f).SetEase(Ease.InOutQuint);
            this.dungeonLocalDataController.CheckStatusOfAllDungeon();
            this.View.dungeonLevelItems.ForEach(item =>
            {
                item.dungeonIdText.text      =  item.dungeonId;
                item.onDungeonSelectBtnClick =  null;
                item.onDungeonSelectBtnClick += this.ShowPopupConfirm;
                //item.gameObject.SetActive(this.dungeonLocalDataController.CheckDungeonIsUnlock(item.dungeonId));
            });
            this.View.ticketValue.text = $"{this.resourceLocalDataController.GetResource(ResourceType.Ticket).Value}";

            return UniTask.CompletedTask;
        }

        private void OnTickResourceValueChange(float value)
        {
            if (this.View == null) return;
            this.View.ticketValue.text = $"{value}";
        }

        private void ShowPopupConfirm(string dungeonId)
        {
            var requireLevel = this.dungeonLocalDataController.GetDungeonRecord(dungeonId).RequireLevel;
            if (requireLevel > this.levelLocalDataController.CurrentLevel.Value)
            {
                this.toastController.ShowToast($"Reach level {requireLevel} in endless mode to unlock dungeon");
                return;
            }
            this.screenManager.OpenScreen<DungeonConfirmPopupPresenter, DungeonConfirmPopupModel>(new() { onConfirmAction = this.CloseView, dungeonId = dungeonId }).Forget();
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