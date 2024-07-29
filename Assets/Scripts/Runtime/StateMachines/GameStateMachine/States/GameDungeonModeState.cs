using Models.LocalData.LocalDataController;
using Runtime.Managers;
using Runtime.Services;
using Runtime.Systems;
using Runtime.Systems.Waves;

namespace Runtime.StateMachines.GameStateMachine.States
{
    public class GameDungeonModeState : BaseGameState
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
        private readonly GenerateGameLevelSystem  generateGameLevelSystem;
        private readonly UserLocalDataController  userLocalDataController;
        private readonly DungeonLocalDataController dungeonLocalDataController;

        public GameDungeonModeState(
            WaveSystem waveSystem,
            SlotManager slotManager,
            TimeCoolDownService timeCoolDownService,
            ArcherManager archerManager,
            HeroManager heroManager,
            SummonerManager summonerManager,
            TowerManager towerManager,
            CastleManager castleManager,
            LevelLocalDataController levelLocalDataController,
            GenerateGameLevelSystem generateGameLevelSystem,
            UserLocalDataController userLocalDataController,
            DungeonLocalDataController dungeonLocalDataController)
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
            this.generateGameLevelSystem = generateGameLevelSystem;
            this.userLocalDataController = userLocalDataController;
            this.dungeonLocalDataController = dungeonLocalDataController;
        }
        public override void Enter()
        {
            this.generateGameLevelSystem.GenerateDungeon(this.dungeonLocalDataController.currentSelectedDungeon);
            this.timeCoolDownService.Resume();
            this.waveSystem.StartDungeonWave(this.dungeonLocalDataController.currentSelectedDungeon);
            this.slotManager.DeActiveAllSlot();
            this.archerManager.ChangeAttackStatusOfAllArcher(true);
            this.heroManager.ChangeAttackStatusOfAllHero(true);
            this.towerManager.ChangeAttackStatusOfAllTower(true);
            this.archerManager.UpdateStatAllArcher();
            this.castleManager.UpdateStatForCurrentCastle();
        }

        public override void Exit() { }
    }
}