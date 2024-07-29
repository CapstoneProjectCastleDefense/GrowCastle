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
        private readonly EnemyManager            enemyManager;
        private readonly SummonerManager         summonerManager;
        private readonly ProjectileManager       projectileManager;
        private readonly ScreenManager           screenManager;
        private readonly UserLocalDataController userLocalDataController;
        private readonly WaveSystem              waveSystem;
        public GameDungeonModeEndState(EnemyManager enemyManager, SummonerManager summonerManager,ProjectileManager projectileManager,ScreenManager screenManager,UserLocalDataController userLocalDataController, WaveSystem waveSystem)
        {
            this.enemyManager            = enemyManager;
            this.summonerManager         = summonerManager;
            this.projectileManager       = projectileManager;
            this.screenManager           = screenManager;
            this.userLocalDataController = userLocalDataController;
            this.waveSystem              = waveSystem;
        }
        public override async void Enter()
        {
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