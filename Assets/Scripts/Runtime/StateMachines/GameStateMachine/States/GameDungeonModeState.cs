using Models.LocalData.LocalDataController;
using Runtime.Managers;
using Runtime.Services;
using Runtime.Systems;
using Runtime.Systems.Waves;

namespace Runtime.StateMachines.GameStateMachine.States
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using Runtime.Signals;
    using Zenject;

    public class GameDungeonModeState : BaseGameState
    {
        private readonly WaveSystem                 waveSystem;
        private readonly SlotManager                slotManager;
        private readonly TimeCoolDownService        timeCoolDownService;
        private readonly ArcherManager              archerManager;
        private readonly HeroManager                heroManager;
        private readonly SummonerManager            summonerManager;
        private readonly TowerManager               towerManager;
        private readonly CastleManager              castleManager;
        private readonly LevelLocalDataController   levelLocalDataController;
        private readonly GenerateGameLevelSystem    generateGameLevelSystem;
        private readonly UserLocalDataController    userLocalDataController;
        private readonly DungeonLocalDataController dungeonLocalDataController;
        private readonly EnemyManager               enemyManager;
        private readonly ScreenManager              screenManager;
        private readonly SignalBus                  suSignalBus;


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
            DungeonLocalDataController dungeonLocalDataController,
            EnemyManager enemyManager,
            ScreenManager screenManager,
            SignalBus suSignalBus)
        {
            this.waveSystem                 = waveSystem;
            this.slotManager                = slotManager;
            this.timeCoolDownService        = timeCoolDownService;
            this.archerManager              = archerManager;
            this.heroManager                = heroManager;
            this.summonerManager            = summonerManager;
            this.towerManager               = towerManager;
            this.castleManager              = castleManager;
            this.levelLocalDataController   = levelLocalDataController;
            this.generateGameLevelSystem    = generateGameLevelSystem;
            this.userLocalDataController    = userLocalDataController;
            this.dungeonLocalDataController = dungeonLocalDataController;
            this.enemyManager               = enemyManager;
            this.screenManager              = screenManager;
            this.suSignalBus                = suSignalBus;
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
            UniTask.Delay(TimeSpan.FromSeconds(1)).ContinueWith(() =>
            {
                this.enemyManager.SpawnBossEnemy(this.dungeonLocalDataController.GetDungeonRecord(this.dungeonLocalDataController.currentSelectedDungeon).BossId);
                this.suSignalBus.Fire<SpawnedBossInDungeon>();
            });
        }

        public override void Exit() { }
    }
}