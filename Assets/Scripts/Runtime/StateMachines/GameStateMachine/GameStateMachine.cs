namespace TheOneStudio.HyperCasual.StateMachines.Game
{
    using System.Collections.Generic;
    using System.Linq;
    using TheOneStudio.HyperCasual.Others.StateMachine.Interface;
    using TheOneStudio.HyperCasual.StateMachines.Game.Interfaces;
    using TheOneStudio.HyperCasual.StateMachines.Game.States;
    using TheOneStudio.UITemplate.UITemplate.Others.StateMachine.Controller;
    using Zenject;

    public class GameStateMachine : StateMachine, IInitializable
    {
        public GameStateMachine(
            List<IGameState> listGameState,
            SignalBus        signalBus
        )
            : base(listGameState.Select(state => state as IState).ToList(), signalBus)
        {
            listGameState.ForEach(e =>
            {
                if (e is BaseGameState state)
                {
                    state.gameStateMachine = this;
                }
            });
        }

        public void Initialize()
        {

        }
    }
}