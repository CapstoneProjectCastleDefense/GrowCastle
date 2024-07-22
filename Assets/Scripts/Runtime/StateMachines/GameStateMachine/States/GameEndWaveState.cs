namespace Runtime.StateMachines.GameStateMachine.States
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Managers;
    using Runtime.Scenes.Popups;

    public class GameEndWaveState : BaseGameState
    {
        private readonly EnemyManager            enemyManager;
        private readonly SummonerManager         summonerManager;
        private readonly ProjectileManager       projectileManager;
        private readonly ScreenManager           screenManager;
        private readonly UserLocalDataController userLocalDataController;
        public GameEndWaveState(EnemyManager enemyManager, SummonerManager summonerManager,ProjectileManager projectileManager,ScreenManager screenManager,UserLocalDataController userLocalDataController)
        {
            this.enemyManager            = enemyManager;
            this.summonerManager         = summonerManager;
            this.projectileManager       = projectileManager;
            this.screenManager           = screenManager;
            this.userLocalDataController = userLocalDataController;
        }
        public override async void Enter()
        {
            this.enemyManager.DisposeAllElement();
            this.summonerManager.DisposeAllElement();
            this.projectileManager.DisposeAllElement();
            await this.screenManager.OpenScreen<EndGamePopupPresenter, EndGamePopupModel>(new EndGamePopupModel() { IsWin = this.userLocalDataController.IsWinCurrentLevel });
        }

        public override void Exit()
        {

        }
    }
}