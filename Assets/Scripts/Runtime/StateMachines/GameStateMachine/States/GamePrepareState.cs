namespace Runtime.StateMachines.GameStateMachine.States
{
    using Runtime.Systems;

    public class GamePrepareState : BaseGameState
    {
        private readonly GenerateGameLevelSystem gameLevelSystem;
        public GamePrepareState(GenerateGameLevelSystem gameLevelSystem) { this.gameLevelSystem = gameLevelSystem; }
        public override void Enter()
        {
            this.gameLevelSystem.GenerateCurrentLevelGame();
        }

        public override void Exit()
        {

        }
    }
}