namespace Runtime.StateMachines.GameStateMachine.States
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using Runtime.Scenes;
    using Runtime.Systems;

    public class GamePrepareState : BaseGameState
    {
        private readonly GenerateGameLevelSystem gameLevelSystem;
        private readonly ScreenManager           screenManager;
        public GamePrepareState(GenerateGameLevelSystem gameLevelSystem, ScreenManager screenManager)
        {
            this.gameLevelSystem = gameLevelSystem;
            this.screenManager   = screenManager;
        }
        public override void Enter()
        {
            this.gameLevelSystem.GenerateCurrentLevelGame();
        }

        public override void Exit()
        {

        }
    }
}