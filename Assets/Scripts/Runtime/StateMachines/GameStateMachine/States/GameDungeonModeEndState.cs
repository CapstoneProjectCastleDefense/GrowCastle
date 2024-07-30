namespace Runtime.StateMachines.GameStateMachine.States
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Managers;
    using Runtime.Scenes.Popups;
    using Runtime.Systems.Waves;

    public class GameDungeonModeEndState : BaseGameState
    {
        private readonly EnemyManager               enemyManager;
        private readonly SummonerManager            summonerManager;
        private readonly ProjectileManager          projectileManager;
        private readonly ScreenManager              screenManager;
        private readonly UserLocalDataController    userLocalDataController;
        private readonly WaveSystem                 waveSystem;
        private readonly DungeonLocalDataController dungeonLocalDataController;
        public GameDungeonModeEndState(EnemyManager enemyManager, SummonerManager summonerManager,ProjectileManager projectileManager,ScreenManager screenManager,UserLocalDataController userLocalDataController, WaveSystem waveSystem, DungeonLocalDataController dungeonLocalDataController)
        {
            this.enemyManager               = enemyManager;
            this.summonerManager            = summonerManager;
            this.projectileManager          = projectileManager;
            this.screenManager              = screenManager;
            this.userLocalDataController    = userLocalDataController;
            this.waveSystem                 = waveSystem;
            this.dungeonLocalDataController = dungeonLocalDataController;
        }
        public override async void Enter()
        {
            this.dungeonLocalDataController.CompleteCurrentDungeon();
            this.enemyManager.DisposeAllElement();
            this.summonerManager.DisposeAllElement();
            this.projectileManager.DisposeAllElement();
            this.waveSystem.ClearDungeon();
            await this.screenManager.OpenScreen<DungeonEndPopupPresenter>();
        }

        public override void Exit()
        {

        }
    }
}