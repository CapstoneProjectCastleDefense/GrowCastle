namespace Runtime.StateMachines.GameStateMachine
{
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.StateMachines.GameStateMachine.Interfaces;
    using Runtime.StateMachines.GameStateMachine.States;
    using Runtime.StateMachines.StateMachineBase.Controller;
    using Runtime.StateMachines.StateMachineBase.Interface;
    using Zenject;

    public class GameStateMachine : StateMachine, IInitializable
    {
        public GameStateMachine(
            List<IGameState> listGameState,
            SignalBus signalBus
        )
            : base(listGameState.Select(state => state as IState).ToList(), signalBus)
        {
            listGameState.ForEach(e =>
            {
                if (e is IHaveStateMachine state)
                {
                    state.StateMachine = this;
                }
            });
        }

        public void Initialize() { this.TransitionTo<GamePrepareState>(); }
    }
}