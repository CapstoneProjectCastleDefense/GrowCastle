namespace Runtime.StateMachines.StateMachineBase.Signals
{
    using Runtime.StateMachines.StateMachineBase.Interface;

    public class OnStateEnterSignal
    {
        public IState State { get; }

        public OnStateEnterSignal(IState state) { this.State = state; }
    }
}