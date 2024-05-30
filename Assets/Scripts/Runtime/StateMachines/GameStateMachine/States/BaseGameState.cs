namespace Runtime.StateMachines.GameStateMachine.States
{
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.Interfaces;

    public abstract class BaseGameState : IGameState
    {

        public GameStateMachine gameStateMachine;

        public abstract void Enter();

        public abstract void Exit();
    }
}