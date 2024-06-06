namespace Runtime.StateMachines.GameStateMachine
{
    using Runtime.StateMachines.GameStateMachine.Interfaces;
    using Runtime.StateMachines.StateMachineBase.Signals;
    using Zenject;

    public class GameStateMachineInstaller : Installer<GameStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<GameStateMachine>().AsSingle().NonLazy();
            this.Container.Bind<IGameState>().To(convention => convention.AllNonAbstractClasses().DerivingFrom<IGameState>()).WhenInjectedInto<GameStateMachine>();
            this.DeclareSignals();
        }

        private void DeclareSignals()
        {
            this.Container.DeclareSignal<OnStateEnterSignal>();
            this.Container.DeclareSignal<OnStateExitSignal>();
        }
    }
}