namespace Runtime.StateMachines.GameStateMachine.States
{
    public class GameEndWaveState : BaseGameState
    {
        public override void Enter()
        {
            this.StateMachine.TransitionTo<GamePrepareState>();
        }

        public override void Exit()
        {

        }
    }
}