namespace Runtime.StateMachines.GameStateMachine.States
{
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Managers;

    public class GameEndWaveState : BaseGameState
    {
        private readonly EnemyManager      enemyManager;
        private readonly SummonerManager   summonerManager;
        private readonly ProjectileManager projectileManager;
        public GameEndWaveState(EnemyManager enemyManager, SummonerManager summonerManager,ProjectileManager projectileManager)
        {
            this.enemyManager      = enemyManager;
            this.summonerManager   = summonerManager;
            this.projectileManager = projectileManager;
        }
        public override void Enter()
        {
            this.enemyManager.DisposeAllElement();
            this.summonerManager.DisposeAllElement();
            this.projectileManager.DisposeAllElement();
            this.StateMachine.TransitionTo<GamePrepareState>();
        }

        public override void Exit()
        {

        }
    }
}