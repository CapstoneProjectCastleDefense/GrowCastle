namespace Runtime.Scenes
{
    using System;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Managers;
    using Runtime.Signals;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class GameplayScreenView : BaseView
    {
        public Image      backGround;
        public Button     startWaveButton;
        public Button     upgradeCastle;
        public Image      castleHealthBar;
        public Image      castleManaBar;
        public GameObject upgradeFiled;
    }

    [ScreenInfo(nameof(GameplayScreenView))]
    public class GameplayScreenPresenter : BaseScreenPresenter<GameplayScreenView>
    {
        private readonly GameStateMachine gameStateMachine;
        private readonly CastleManager    castleManager;
        private readonly SignalBus        signalBus;
        public GameplayScreenPresenter(SignalBus signalBus, GameStateMachine gameStateMachine, CastleManager castleManager) : base(signalBus) {
            this.gameStateMachine = gameStateMachine;
            this.castleManager    = castleManager;
            this.signalBus        = signalBus;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.OpenViewAsync().Forget();
            this.signalBus.Subscribe<UpdateCastleStatSignal>(this.OnCastleStatChange);
            this.View.startWaveButton.onClick.AddListener(this.OnStartWaveButtonClick);
            this.View.upgradeCastle.onClick.AddListener(this.OnUpgradeCastleButtonClick);
        }

        private void OnCastleStatChange(UpdateCastleStatSignal signal)
        {
            var a = signal.CastleStats.GetStat<float>(StatEnum.MaxHealth);
            var b = signal.CastleStats.GetStat<float>(StatEnum.Health);
            this.View.castleHealthBar.DOFillAmount(signal.CastleStats.GetStat<float>(StatEnum.Health)*1.0f/signal.CastleStats.GetStat<float>(StatEnum.MaxHealth),0.1f);
            //this.View.castleManaBar.fillAmount = signal.CastleStats.GetStat<float>(Sat)
        }

        private void OnUpgradeCastleButtonClick() {
            this.castleManager.UpgradeCastle();
        }

        private void OnStartWaveButtonClick()
        {
            this.gameStateMachine.TransitionTo<GameStartWaveState>();
            this.View.upgradeFiled.gameObject.SetActive(false);
            this.View.startWaveButton.gameObject.SetActive(false);
        }

        public override UniTask BindData()
        {
            UniTask.Delay(TimeSpan.FromSeconds(1)).ContinueWith(() =>
            {
                this.View.backGround.DOFade(0, 3).SetEase(Ease.OutQuad);
            });
            return UniTask.CompletedTask;
        }
    }
}