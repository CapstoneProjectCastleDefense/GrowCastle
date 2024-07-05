namespace Runtime.Scenes
{
    using System;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
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
    using Runtime.StateMachines.StateMachineBase.Signals;
    using Runtime.Scenes.Popups;
    using Sirenix.Serialization;
    using System.Threading.Tasks;
    using System.Collections.Generic;

    public class GameplayScreenView : BaseView
    {
        public Image backGround;
        public Button startWaveButton;
        public Button upgradeCastle;
        public Button upgradeArcher;
        public Button dailyRewardButton;
        public Image castleHealthBar;
        public Image castleManaBar;
        public Image waveBar;
        public GameObject waveIndicator;
        public GameObject upgradeField;

        public TextMeshProUGUI castleCoinUpgradeValue;
        public TextMeshProUGUI archerCoinUpgradeValue;
        public TextMeshProUGUI waveValue;
        public TextMeshProUGUI goldValue;
        public TextMeshProUGUI diamondValue;
    }

    [ScreenInfo(nameof(GameplayScreenView))]
    public class GameplayScreenPresenter : BaseScreenPresenter<GameplayScreenView>
    {
        private readonly GameStateMachine gameStateMachine;
        private readonly CastleManager castleManager;
        private readonly ArcherManager archerManager;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly ScreenManager screenManager;
        private readonly SignalBus signalBus;
        public GameplayScreenPresenter(
            SignalBus signalBus,
            GameStateMachine gameStateMachine,
            CastleManager castleManager,
            ArcherManager archerManager,
            ResourceLocalDataController resourceLocalDataController,
            ScreenManager screenManager)
            : base(signalBus) {
            this.gameStateMachine = gameStateMachine;
            this.castleManager = castleManager;
            this.archerManager = archerManager;
            this.resourceLocalDataController = resourceLocalDataController;
            this.screenManager = screenManager;
            this.signalBus = signalBus;
        }

        protected override void OnViewReady() {
            base.OnViewReady();
            this.OpenViewAsync().Forget();
            this.signalBus.Subscribe<UpdateCastleStatSignal>(this.OnCastleStatChange);
            this.signalBus.Subscribe<OnStateEnterSignal>(this.OnEnterNewGameState);
            this.View.startWaveButton.onClick.AddListener(this.OnStartWaveButtonClick);
            this.View.upgradeCastle.onClick.AddListener(this.OnUpgradeCastleButtonClick);
            this.View.upgradeArcher.onClick.AddListener(this.OnUpgradeArcherButtonClick);
            this.View.dailyRewardButton.onClick.AddListener(this.OnDailyRewardClick);

            this.resourceLocalDataController.GetResource(ResourceType.Gold).Subscribe(this.OnGoldValueChange);
            this.resourceLocalDataController.GetResource(ResourceType.Diamond).Subscribe(this.OnDiamondValueChange);
            this.View.waveIndicator.SetActive(false);

        }

        private void OnCastleStatChange(UpdateCastleStatSignal signal) {
            this.View.castleHealthBar.DOFillAmount(signal.CastleStats.GetStat<float>(StatEnum.Health) * 1.0f / signal.CastleStats.GetStat<float>(StatEnum.MaxHealth), 0.1f);
        }

        private void OnUpgradeCastleButtonClick() {
            this.castleManager.UpgradeCastle();
        }

        private void OnUpgradeArcherButtonClick() { this.archerManager.UpgradeArcher(); }
        private async void OnDailyRewardClick() { await this.screenManager.OpenScreen<DailyRewardPopupPresenter>(); }

        private void OnStartWaveButtonClick() {
            this.gameStateMachine.TransitionTo<GameStartWaveState>();
        }

        private async void OnEnterNewGameState(OnStateEnterSignal signal) {
            float fadeTime = 0.7f;
            switch (signal.State)
            {
                case GamePrepareState:
                    //this.View.backGround.DOFade(1, 0);
                    //this.View.backGround.DOFade(0, 3).SetEase(Ease.OutQuad);

                    this.View.upgradeField.GetComponent<RectTransform>().DOAnchorPosX(-420f, fadeTime).SetEase(Ease.OutElastic);
                    this.View.startWaveButton.gameObject.GetComponent<RectTransform>().DOAnchorPosY(178f, fadeTime).SetEase(Ease.OutElastic);
                    this.View.dailyRewardButton.gameObject.GetComponent<RectTransform>().DOAnchorPosY(157f, fadeTime).SetEase(Ease.OutElastic);
                    this.View.waveIndicator.SetActive(false);
                    return;
                case GameStartWaveState:
                    this.View.upgradeField.GetComponent<RectTransform>().DOAnchorPosX(1000f, fadeTime).SetEase(Ease.InOutQuint);
                    this.View.startWaveButton.gameObject.GetComponent<RectTransform>().DOAnchorPosY(-1000f, fadeTime).SetEase(Ease.InOutQuint);
                    this.View.dailyRewardButton.gameObject.GetComponent<RectTransform>().DOAnchorPosY(-1000f, fadeTime).SetEase(Ease.InOutQuint);
                    this.View.waveIndicator.SetActive(true);
                    break;
            }
        }

        private void OnGoldValueChange(float value) => this.View.goldValue.text = $"{value}";
        private void OnDiamondValueChange(float value) => this.View.diamondValue.text = $"{value}";

        public override UniTask BindData() {
            this.View.goldValue.text = $"{this.resourceLocalDataController.GetResource(ResourceType.Gold).Value}";
            this.View.diamondValue.text = $"{this.resourceLocalDataController.GetResource(ResourceType.Diamond).Value}";
            UniTask.Delay(TimeSpan.FromSeconds(1)).ContinueWith(() => { this.View.backGround.DOFade(0, 3).SetEase(Ease.OutQuad); });
            return UniTask.CompletedTask;
        }

    }
}