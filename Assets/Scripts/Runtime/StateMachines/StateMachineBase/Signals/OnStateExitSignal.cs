namespace Runtime.StateMachines.StateMachineBase.Signals
{
    using Runtime.StateMachines.StateMachineBase.Interface;

    public class OnStateExitSignal
    {
        public IState State { get; }

        public OnStateExitSignal(IState state) { this.State = state; }
    }
}