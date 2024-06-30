namespace Runtime.StateMachines.GameStateMachine.States
{
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.Systems.Waves;

    public class GameStartWaveState : BaseGameState
    {
        private readonly WaveSystem          waveSystem;
        private readonly SlotManager         slotManager;
        private readonly TimeCoolDownService timeCoolDownService;
        private readonly ArcherManager       archerManager;
        private readonly HeroManager         heroManager;
        private readonly TowerManager towerManager;

        public GameStartWaveState(WaveSystem waveSystem, SlotManager slotManager, TimeCoolDownService timeCoolDownService, ArcherManager archerManager, HeroManager heroManager, TowerManager towerManager) {
            this.waveSystem = waveSystem;
            this.slotManager = slotManager;
            this.timeCoolDownService = timeCoolDownService;
            this.archerManager = archerManager;
            this.heroManager = heroManager;
            this.towerManager = towerManager;
        }
        public override void Enter()
        {
            this.timeCoolDownService.Resume();
            this.waveSystem.StartCurrentWave(1);
            this.slotManager.DeActiveAllSlot();
            this.archerManager.ChangeAttackStatusOfAllArcher(true);
            this.heroManager.ChangeAttackStatusOfAllHero(true);
            this.towerManager.ChangeAttackStatusOfAllTower(true);
        }

        public override void Exit()
        {

        }
    }
}