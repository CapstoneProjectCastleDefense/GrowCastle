namespace TheOneStudio.HyperCasual.StateMachines.Game.States
{
    using TheOneStudio.HyperCasual.StateMachines.Game.Interfaces;

    public abstract class BaseGameState : IGameState
    {

        public GameStateMachine gameStateMachine;

        public abstract void Enter();

        public abstract void Exit();
    }
}