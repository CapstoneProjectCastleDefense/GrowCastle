namespace Runtime.Scenes
{
    using System;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Managers;
    using Runtime.Signals;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;
    using R3;

    public class GameplayScreenView : BaseView
    {
        public Image      backGround;
        public Button     startWaveButton;
        public Button     upgradeCastle;
        public Button     upgradeArcher;
        public Image      castleHealthBar;
        public Image      castleManaBar;
        public GameObject upgradeFiled;

        public TextMeshProUGUI goldValue;
        public TextMeshProUGUI diamondValue;
    }

    [ScreenInfo(nameof(GameplayScreenView))]
    public class GameplayScreenPresenter : BaseScreenPresenter<GameplayScreenView>
    {
        private readonly GameStateMachine            gameStateMachine;
        private readonly CastleManager               castleManager;
        private readonly ArcherManager               archerManager;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly SignalBus                   signalBus;
        public GameplayScreenPresenter(SignalBus signalBus, GameStateMachine gameStateMachine, CastleManager castleManager, ArcherManager archerManager, ResourceLocalDataController resourceLocalDataController)
            : base(signalBus)
        {
            this.gameStateMachine            = gameStateMachine;
            this.castleManager               = castleManager;
            this.archerManager               = archerManager;
            this.resourceLocalDataController = resourceLocalDataController;
            this.signalBus                   = signalBus;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.OpenViewAsync().Forget();
            this.signalBus.Subscribe<UpdateCastleStatSignal>(this.OnCastleStatChange);
            this.View.startWaveButton.onClick.AddListener(this.OnStartWaveButtonClick);
            this.View.upgradeCastle.onClick.AddListener(this.OnUpgradeCastleButtonClick);
            this.View.upgradeArcher.onClick.AddListener(this.OnUpgradeArcherButtonClick);

            this.resourceLocalDataController.GetResource(ResourceType.Gold).Subscribe(this.OnGoldValueChange);
            this.resourceLocalDataController.GetResource(ResourceType.Diamond).Subscribe(this.OnDiamondValueChange);
        }

        private void OnCastleStatChange(UpdateCastleStatSignal signal)
        {
            this.View.castleHealthBar.DOFillAmount(signal.CastleStats.GetStat<float>(StatEnum.Health) * 1.0f / signal.CastleStats.GetStat<float>(StatEnum.MaxHealth), 0.1f);
        }

        private void OnUpgradeCastleButtonClick() { this.castleManager.UpgradeCastle(); }

        private void OnUpgradeArcherButtonClick() { this.archerManager.UpgradeArcher(); }

        private void OnStartWaveButtonClick()
        {
            this.gameStateMachine.TransitionTo<GameStartWaveState>();
            this.View.upgradeFiled.gameObject.SetActive(false);
            this.View.startWaveButton.gameObject.SetActive(false);
        }

        private void OnGoldValueChange(float value) => this.View.goldValue.text = $"{value}";
        private void OnDiamondValueChange(float value) => this.View.diamondValue.text = $"{value}";

        public override UniTask BindData()
        {
            this.View.goldValue.text    = $"{this.resourceLocalDataController.GetResource(ResourceType.Gold).Value}";
            this.View.diamondValue.text = $"{this.resourceLocalDataController.GetResource(ResourceType.Diamond).Value}";
            UniTask.Delay(TimeSpan.FromSeconds(1)).ContinueWith(() => { this.View.backGround.DOFade(0, 3).SetEase(Ease.OutQuad); });
            return UniTask.CompletedTask;
        }
    }
}