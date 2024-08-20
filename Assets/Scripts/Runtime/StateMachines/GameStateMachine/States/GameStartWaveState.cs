namespace Runtime.StateMachines.GameStateMachine.States
{
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Managers;
    using Runtime.Scenes;
    using Runtime.Services;
    using Runtime.Systems.Waves;

    public class GameStartWaveState : BaseGameState
    {
        private readonly WaveSystem               waveSystem;
        private readonly SlotManager              slotManager;
        private readonly TimeCoolDownService      timeCoolDownService;
        private readonly ArcherManager            archerManager;
        private readonly HeroManager              heroManager;
        private readonly SummonerManager          summonerManager;
        private readonly TowerManager             towerManager;
        private readonly CastleManager            castleManager;
        private readonly LevelLocalDataController levelLocalDataController;

        public GameStartWaveState(
            WaveSystem waveSystem,
            SlotManager slotManager,
            TimeCoolDownService timeCoolDownService,
            ArcherManager archerManager,
            HeroManager heroManager,
            SummonerManager summonerManager,
            TowerManager towerManager,
            CastleManager castleManager,
            LevelLocalDataController levelLocalDataController)
        {
            this.waveSystem               = waveSystem;
            this.slotManager              = slotManager;
            this.timeCoolDownService      = timeCoolDownService;
            this.archerManager            = archerManager;
            this.heroManager              = heroManager;
            this.summonerManager          = summonerManager;
            this.towerManager             = towerManager;
            this.castleManager            = castleManager;
            this.levelLocalDataController = levelLocalDataController;
        }
        public override void Enter()
        {
            this.timeCoolDownService.Resume();
            this.waveSystem.StartCurrentWave(this.levelLocalDataController.CurrentLevelValue);
            this.slotManager.DeActiveAllSlot();
            this.archerManager.ChangeAttackStatusOfAllArcher(true);
            this.heroManager.ChangeAttackStatusOfAllHero(true);
            this.towerManager.ChangeAttackStatusOfAllTower(true);
            this.archerManager.UpdateStatAllArcher();
            this.castleManager.UpdateStatForCurrentCastle();
            this.slotManager.UpdateStatEffectForAllHero();
        }

        public override void Exit() { }
    }
}