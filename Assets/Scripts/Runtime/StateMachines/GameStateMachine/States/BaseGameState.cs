namespace Runtime.StateMachines.GameStateMachine.States
{
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.Interfaces;
    using Runtime.StateMachines.StateMachineBase.Interface;

    public abstract class BaseGameState : IGameState, IHaveStateMachine
    {
        public abstract void Enter();

        public abstract void          Exit();
        public          IStateMachine StateMachine { get; set; }
    }
}