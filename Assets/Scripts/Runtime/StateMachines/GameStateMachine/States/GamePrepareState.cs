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
        public override async void Enter()
        {
            //await this.screenManager.OpenScreen<GameplayScreenPresenter>();
            this.gameLevelSystem.GenerateCurrentLevelGame();
            this.gameStateMachine.TransitionTo<GameStartWaveState>();
        }

        public override void Exit()
        {

        }
    }
}