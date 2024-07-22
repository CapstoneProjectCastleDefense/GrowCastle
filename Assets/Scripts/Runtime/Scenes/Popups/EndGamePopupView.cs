namespace Runtime.Scenes.Popups
{
    using System;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using UnityEngine;
    using Zenject;

    public class EndGamePopupModel
    {
        public bool IsWin;
    }

    public class EndGamePopupView : BaseView
    {
        public GameObject winGameObject;
        public GameObject loseGameObject;
        public Transform  startPos;
        public Transform  endPos;
    }

    [PopupInfo(nameof(EndGamePopupView), isOverlay: true)]
    public class EndGamePopupPresenter : BasePopupPresenter<EndGamePopupView, EndGamePopupModel>
    {
        private readonly GameStateMachine gameStateMachine;
        public EndGamePopupPresenter(SignalBus signalBus, ILogService logService,GameStateMachine gameStateMachine)
            : base(signalBus, logService)
        {
            this.gameStateMachine = gameStateMachine;
        }
        public override UniTask BindData(EndGamePopupModel popupModel)
        {
            var position = this.View.startPos.position;
            this.View.loseGameObject.transform.position = position;
            this.View.winGameObject.transform.position  = position;
            this.View.loseGameObject.SetActive(!this.Model.IsWin);
            this.View.winGameObject.SetActive(this.Model.IsWin);
            
            var animObj = this.Model.IsWin ? this.View.winGameObject : this.View.loseGameObject;
            this.DoAnimation(animObj);
            
            return UniTask.CompletedTask;
        }
        private void DoAnimation(GameObject gameObject)
        {
            gameObject.transform.position = this.View.startPos.position;
            gameObject.transform.DOMove(this.View.endPos.position, 1f).SetEase(Ease.OutElastic).onComplete +=async () =>
            {
                await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
                this.CloseView();
                this.gameStateMachine.TransitionTo<GamePrepareState>();
            };
        }
    }
}