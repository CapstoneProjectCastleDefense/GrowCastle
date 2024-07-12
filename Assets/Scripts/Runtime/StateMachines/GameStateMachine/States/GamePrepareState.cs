namespace Runtime.StateMachines.GameStateMachine.States
{
    using Codice.CM.Common;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using Runtime.Managers;
    using Runtime.Scenes;
    using Runtime.Services;
    using Runtime.Systems;

    public class GamePrepareState : BaseGameState
    {
        private readonly GenerateGameLevelSystem gameLevelSystem;
        private readonly ScreenManager           screenManager;
        private readonly TimeCoolDownService     timeCoolDownService;
        private readonly ArcherManager           archerManager;
        private readonly HeroManager             heroManager;
        private readonly TowerManager            towerManager;
        private readonly CastleManager           castleManager;
        private readonly SlotManager             slotManager;

        public GamePrepareState(GenerateGameLevelSystem gameLevelSystem, ScreenManager screenManager, TimeCoolDownService timeCoolDownService, ArcherManager archerManager, HeroManager heroManager, TowerManager towerManager, CastleManager castleManager, SlotManager slotManager) {
            this.gameLevelSystem = gameLevelSystem;
            this.screenManager = screenManager;
            this.timeCoolDownService = timeCoolDownService;
            this.archerManager = archerManager;
            this.heroManager = heroManager;
            this.towerManager = towerManager;
            this.castleManager = castleManager;
            this.slotManager = slotManager;
        }
        public override void Enter()
        {
            this.timeCoolDownService.Pause();
            this.gameLevelSystem.GenerateCurrentLevelGame();
            this.archerManager.ChangeAttackStatusOfAllArcher(false);
            this.heroManager.ChangeAttackStatusOfAllHero(false);
            this.towerManager.ChangeAttackStatusOfAllTower(false);
            this.castleManager.ResetCurrentCastleHealthAndMana();
            this.slotManager.ActiveAllSlot();
        }

        public override void Exit() { }
    }
}