namespace Runtime.Scenes
{
    using System;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Runtime.Managers;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using UnityEngine.UI;
    using Zenject;

    public class GameplayScreenView : BaseView
    {
        public Image  backGround;
        public Button startWaveButton;
        public Button upgradeCastle;
    }

    [ScreenInfo(nameof(GameplayScreenView))]
    public class GameplayScreenPresenter : BaseScreenPresenter<GameplayScreenView>
    {
        private readonly GameStateMachine gameStateMachine;
        private readonly CastleManager castleManager;
        public GameplayScreenPresenter(SignalBus signalBus, GameStateMachine gameStateMachine, CastleManager castleManager) : base(signalBus) {
            this.gameStateMachine = gameStateMachine;
            this.castleManager = castleManager;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.OpenViewAsync().Forget();
            this.View.startWaveButton.onClick.AddListener(this.OnStartWaveButtonClick);
            this.View.upgradeCastle.onClick.AddListener(this.OnUpgradeCastleButtonClick);
        }

        private void OnUpgradeCastleButtonClick() {
            this.castleManager.UpgradeCastle();
        }

        private void OnStartWaveButtonClick()
        {
            this.gameStateMachine.TransitionTo<GameStartWaveState>();
            this.View.upgradeCastle.gameObject.SetActive(false);
        }

        public override UniTask BindData()
        {
            UniTask.Delay(TimeSpan.FromSeconds(1)).ContinueWith(() =>
            {
                this.View.backGround.DOFade(0, 2).SetEase(Ease.OutQuad);
            });
            return UniTask.CompletedTask;
        }
    }
}