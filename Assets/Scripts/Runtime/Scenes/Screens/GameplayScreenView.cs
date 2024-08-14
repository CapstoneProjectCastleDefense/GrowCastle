namespace Runtime.Scenes.Screens
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using R3;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Managers;
    using Runtime.Scenes.Popups;
    using Runtime.Services;
    using Runtime.Signals;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using Runtime.StateMachines.StateMachineBase.Signals;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class GameplayScreenView : BaseView
    {
        public Image backGround;

        public Button       startWaveButton;
        public Button       upgradeCastle;
        public Button       upgradeArcher;
        public Button       dailyRewardButton;
        public Button       talentButton;
        public Button       questButton;
        public Button       inventoryButton;
        public Button       chestButton;
        public Button       dungeonModeBtn;
        public Button       speedRunX2;
        public Button       settingBtn;
        public List<Button> comingSoonBtn;

        public Image castleHealthBar;
        public Image castleManaBar;
        public Image waveBar;
        public Image userExpBar;
        public Image bossHealthBar;

        public GameObject waveIndicator;
        public GameObject upgradeField;
        public GameObject bossHealth;

        public TextMeshProUGUI castleCoinUpgradeValue;
        public TextMeshProUGUI archerCoinUpgradeValue;
        public TextMeshProUGUI castleCurrentLevel;
        public TextMeshProUGUI archerCurrentLevel;
        public TextMeshProUGUI waveValue;
        public TextMeshProUGUI goldValue;
        public TextMeshProUGUI diamondValue;
        public TextMeshProUGUI healthCurrentValue;
        public TextMeshProUGUI manaCurrentValue;
        public TextMeshProUGUI userLevelValue;
        public TextMeshProUGUI bossHealthValue;
        public TextMeshProUGUI timeSpeedValue;

        public GameObject topObject;
        public GameObject midObject;
        public GameObject bottomObject;
        public Transform  startPosBottom;
        public Transform  endPosBottom;
        public Transform  startPosMid;
        public Transform  endPosMid;
    }

    [ScreenInfo(nameof(GameplayScreenView))]
    public class GameplayScreenPresenter : BaseScreenPresenter<GameplayScreenView>
    {
        private readonly GameStateMachine            gameStateMachine;
        private readonly CastleManager               castleManager;
        private readonly ArcherManager               archerManager;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly ScreenManager               screenManager;
        private readonly LevelLocalDataController    levelLocalDataController;
        private readonly CastleLocalDataController   castleLocalDataController;
        private readonly ArcherLocalDataController   archerLocalDataController;
        private readonly UserLocalDataController     userLocalDataController;
        private readonly FeatureLocalDataController  featureLocalDataController;
        private readonly EnemyManager                enemyManager;
        private readonly ToastController             toastController;
        private readonly SignalBus                   signalBus;

        private Vector3 bottomPos;
        private Vector3 midPos;
        public GameplayScreenPresenter(
            SignalBus signalBus,
            GameStateMachine gameStateMachine,
            CastleManager castleManager,
            ArcherManager archerManager,
            ResourceLocalDataController resourceLocalDataController,
            ScreenManager screenManager,
            LevelLocalDataController levelLocalDataController,
            CastleLocalDataController castleLocalDataController,
            ArcherLocalDataController archerLocalDataController,
            UserLocalDataController userLocalDataController,
            FeatureLocalDataController featureLocalDataController,
            EnemyManager enemyManager,
            ToastController toastController)
            : base(signalBus)
        {
            this.gameStateMachine            = gameStateMachine;
            this.castleManager               = castleManager;
            this.archerManager               = archerManager;
            this.resourceLocalDataController = resourceLocalDataController;
            this.screenManager               = screenManager;
            this.levelLocalDataController    = levelLocalDataController;
            this.castleLocalDataController   = castleLocalDataController;
            this.archerLocalDataController   = archerLocalDataController;
            this.userLocalDataController     = userLocalDataController;
            this.featureLocalDataController  = featureLocalDataController;
            this.enemyManager                = enemyManager;
            this.toastController             = toastController;
            this.signalBus                   = signalBus;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.OpenViewAsync().Forget();
            AudioService.Instance.PlayPlayList("bgm");
            this.bottomPos = this.View.bottomObject.transform.position;
            this.midPos    = this.View.midObject.transform.position;
            this.signalBus.Subscribe<UpdateCastleStatSignal>(this.OnCastleStatChange);
            this.signalBus.Subscribe<OnStateEnterSignal>(this.OnEnterNewGameState);
            this.signalBus.Subscribe<SpawnedBossInDungeon>(this.OnStartDungeon);

            this.View.startWaveButton.onClick.AddListener(this.OnStartWaveButtonClick);
            this.View.upgradeCastle.onClick.AddListener(this.OnUpgradeCastleButtonClick);
            this.View.upgradeArcher.onClick.AddListener(this.OnUpgradeArcherButtonClick);
            this.View.dailyRewardButton.onClick.AddListener(this.OnDailyRewardClick);
            this.View.talentButton.onClick.AddListener(this.OnTalentBtnClick);
            this.View.questButton.onClick.AddListener(this.OnQuestBtnClick);
            this.View.inventoryButton.onClick.AddListener(this.OnInventoryBtnClick);
            this.View.chestButton.onClick.AddListener(this.OnChestBtnClick);
            this.View.dungeonModeBtn.onClick.AddListener(this.OnDungeonBtnClick);
            this.View.speedRunX2.onClick.AddListener(this.OnSpeedupClick);
            this.View.settingBtn.onClick.AddListener(this.OnSettingBtnClick);
            this.View.comingSoonBtn.ForEach(button => button.onClick.AddListener(this.ShowComingSoon));

            this.resourceLocalDataController.GetResource(ResourceType.Gold).Subscribe(this.OnGoldValueChange);
            this.resourceLocalDataController.GetResource(ResourceType.Diamond).Subscribe(this.OnDiamondValueChange);
            this.View.waveIndicator.SetActive(false);

            this.levelLocalDataController.CurrentLevel.Subscribe(this.UpdateTextValue);
            this.levelLocalDataController.CurrentLevel.Subscribe(this.OnQuestFeatureUnlock);
            this.levelLocalDataController.CurrentLevel.Subscribe(this.OnTalentFeatureUnlock);

            this.castleLocalDataController.GetStats(StatEnum.Health).Subscribe(this.OnCastleHealthChange);
            this.castleLocalDataController.GetStats(StatEnum.Mana).Subscribe(this.OnCastleManaChange);

            this.View.castleCurrentLevel.text = this.castleLocalDataController.GetCurrentUpgradeLevel().ToString();
            this.View.archerCurrentLevel.text = this.archerLocalDataController.GetCurrentUpgradeLevel().ToString();

            this.resourceLocalDataController.GetResource(ResourceType.Exp).Subscribe(this.OnUserExpUpdate);
            this.userLocalDataController.GetCurrentUserLevel.Subscribe(this.OnUserLevelUpdate);
        }

        private void ShowComingSoon() { this.toastController.ShowToast("Feature is coming soon"); }
        private void OnSpeedupClick()
        {
            var currentTimeSpeed = Time.timeScale;
            Time.timeScale                = 3 - currentTimeSpeed;
            this.View.timeSpeedValue.text = $"x{3 - currentTimeSpeed} speed";
        }

        #region Feature

        private void UpdateTextValue(int value) { this.View.waveValue.text = $"Level {value}"; }
        private void OnQuestFeatureUnlock(int value)
        {
            if (this.featureLocalDataController.CheckFeatureIsUnlock(FeatureName.Quest, value))
            {
                this.View.questButton.gameObject.SetActive(true);
            }
        }

        private void OnTalentFeatureUnlock(int value)
        {
            if (this.featureLocalDataController.CheckFeatureIsUnlock(FeatureName.Talent, value))
            {
                this.View.talentButton.gameObject.SetActive(true);
            }
        }

        #endregion

        private async void OnSettingBtnClick()
        {
            Debug.Log("settings_btn_clicked");
            await this.screenManager.OpenScreen<SettingScreenPresenter>();
        }
        private async void OnDungeonBtnClick()            { await this.screenManager.OpenScreen<DungeonSelectLevelPopupPresenter>(); }
        private async void OnChestBtnClick()              { await this.screenManager.OpenScreen<ChestPopupPresenter>(); }
        private async void OnQuestBtnClick()              { await this.screenManager.OpenScreen<QuestPopupPresenter>(); }
        private       void OnInventoryBtnClick()          { this.screenManager.OpenScreen<ItemInventoryPopupPresenter, ItemInventoryPopupModel>(new(null, null, null)).Forget(); }
        private async void OnTalentBtnClick()             { await this.screenManager.OpenScreen<TalentPopupPresenter>(); }
        private       void OnUserExpUpdate(float value)   { this.View.userExpBar.DOFillAmount(value / this.resourceLocalDataController.GetCurrentTargetExpToLevelUp(), 0.1f); }
        private       void OnUserLevelUpdate(float value) { this.View.userLevelValue.text = $"Level {value}"; }

        private void OnCastleStatChange(UpdateCastleStatSignal signal)
        {
            this.View.castleHealthBar.DOFillAmount(signal.CastleStats.GetStat<float>(StatEnum.Health) * 1.0f / signal.CastleStats.GetStat<float>(StatEnum.MaxHealth),
                0.1f);
            this.View.castleManaBar.DOFillAmount(signal.CastleStats.GetStat<float>(StatEnum.Mana) * 1.0f / signal.CastleStats.GetStat<float>(StatEnum.MaxMana), 0.1f);
        }

        private void OnUpgradeCastleButtonClick()
        {
            this.castleManager.UpgradeCastle();
            this.View.castleCoinUpgradeValue.text = this.castleLocalDataController.GetGoldToUpgrade().ToString("F0");
            this.View.castleCurrentLevel.text     = this.castleLocalDataController.GetCurrentUpgradeLevel().ToString();
        }

        private void OnUpgradeArcherButtonClick()
        {
            this.archerManager.UpgradeArcher();
            this.View.archerCoinUpgradeValue.text = this.archerLocalDataController.GetGoldNeedToUpgrade().ToString(CultureInfo.InvariantCulture);
            this.View.archerCurrentLevel.text     = this.archerLocalDataController.GetCurrentUpgradeLevel().ToString();
        }
        private async void OnDailyRewardClick() { await this.screenManager.OpenScreen<DailyRewardPopupPresenter>(); }

        private void OnStartWaveButtonClick()
        {
            Debug.Log("start_wave_btn_clicked");
            this.gameStateMachine.TransitionTo<GameStartWaveState>();
        }

        private void OnEnterNewGameState(OnStateEnterSignal signal)
        {
            this.View.bossHealth.SetActive(false);
            Time.timeScale                = 1;
            this.View.timeSpeedValue.text = "x1 speed";
            this.View.speedRunX2.gameObject.SetActive(false);
            switch (signal.State)
            {
                case GamePrepareState:
                    this.DoPrepareStateAnim(1f);
                    return;
                case GameStartWaveState:
                    this.DoStartWaveAnim(1f);
                    this.View.speedRunX2.gameObject.SetActive(true);
                    break;
                case GameDungeonModeState:
                    this.DoStartWaveAnim(1f);
                    this.View.waveIndicator.SetActive(false);
                    this.View.speedRunX2.gameObject.SetActive(true);
                    break;
            }
        }

        private void OnStartDungeon()
        {
            this.View.bossHealth.SetActive(true);
            this.enemyManager.CurrentBossHealth.Subscribe(this.OnUpdateBossHealth);
        }

        private void OnUpdateBossHealth(float bossHealth)
        {
            if (bossHealth <= 0)
            {
                bossHealth = 0;
                this.gameStateMachine.TransitionTo<GameDungeonModeEndState>();
            }

            this.View.bossHealthBar.DOFillAmount(bossHealth / this.enemyManager.MaxBossHealth, 0.01f);
            this.View.bossHealthValue.text = $"{bossHealth} / {this.enemyManager.MaxBossHealth}";
        }


        private void DoPrepareStateAnim(float fadeTime)
        {
            this.View.midObject.transform.DOMove(this.midPos, fadeTime).SetEase(Ease.InOutQuint);
            this.View.bottomObject.transform.DOMove(this.bottomPos, fadeTime).SetEase(Ease.InOutQuint);
            this.View.waveIndicator.SetActive(false);
        }

        private void DoStartWaveAnim(float fadeTime)
        {
            this.View.midObject.transform.DOMove(this.View.endPosMid.position, fadeTime).SetEase(Ease.InOutQuint);
            this.View.bottomObject.transform.DOMove(this.View.endPosBottom.position, fadeTime).SetEase(Ease.InOutQuint);
            this.View.waveIndicator.SetActive(true);
        }

        private void OnGoldValueChange(float value)    => this.View.goldValue.text = $"{value:F0}";
        private void OnDiamondValueChange(float value) => this.View.diamondValue.text = $"{value:F0}";
        private void OnCastleManaChange(float value)   => this.View.manaCurrentValue.text = $"{value:F0}";
        private void OnCastleHealthChange(float value) => this.View.healthCurrentValue.text = $"{value:F0}";
        public override UniTask BindData()
        {
            this.View.goldValue.text              = $"{this.resourceLocalDataController.GetResource(ResourceType.Gold).Value}";
            this.View.diamondValue.text           = $"{this.resourceLocalDataController.GetResource(ResourceType.Diamond).Value}";
            this.View.castleCoinUpgradeValue.text = this.castleLocalDataController.GetGoldToUpgrade().ToString(CultureInfo.InvariantCulture);
            this.View.archerCoinUpgradeValue.text = this.archerLocalDataController.GetGoldNeedToUpgrade().ToString(CultureInfo.InvariantCulture);
            this.View.timeSpeedValue.text         = "x1 speed";
            this.InitFeatureStatus();
            UniTask.Delay(TimeSpan.FromSeconds(1)).ContinueWith(() => { this.View.backGround.DOFade(0, 3).SetEase(Ease.OutQuad); });
            return UniTask.CompletedTask;
        }

        private void InitFeatureStatus()
        {
            this.View.questButton.gameObject.SetActive(this.featureLocalDataController.GetFeatureData(FeatureName.Quest).Value);
            this.View.talentButton.gameObject.SetActive(this.featureLocalDataController.GetFeatureData(FeatureName.Talent).Value);
        }
    }
}