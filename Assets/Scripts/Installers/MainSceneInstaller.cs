namespace Installers
{
    using GameFoundation.Scripts.Utilities.Extension;
    using Runtime.Elements.Entities.Castles;
    using Runtime.Managers;
    using Runtime.Managers.Base;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.Interfaces;
    using Runtime.StateMachines.StateMachineBase.Controller;
    using Runtime.StateMachines.StateMachineBase.Interface;
    using Zenject;

    public class MainSceneInstaller : MonoInstaller<MainSceneInstaller>
    {
        public override void InstallBindings()
        {
            this.BindStateMachine<IGameState,GameStateMachine>();
            this.BindAllManager();
            this.BindElement();
            GameStateMachineInstaller.Install(this.Container);
        }

        private void BindElement()
        {
            this.Container.BindFactory<CastleModel, CastlePresenter, CastlePresenter.Factory>().AsCached()
                .WhenInjectedInto<CastleManager>();
        }

        private void BindStateMachine<TState, TStateMachine>() where TState : IState where TStateMachine : StateMachine
        {
            this.Container.Bind<IState>()
                .To(convention => convention.AllNonAbstractClasses().DerivingFrom<TState>())
                .AsSingle()
                .WhenInjectedInto<TStateMachine>()
                .NonLazy();
            this.Container.BindInterfacesAndSelfTo<TStateMachine>().AsSingle().NonLazy();
        }

        private void BindAllManager()
        {
            foreach (var type in ReflectionUtils.GetAllDerivedTypes<IElementManager>())
            {
                if(!type.IsAbstract) this.Container.BindInterfacesAndSelfTo(type).AsCached().NonLazy();
            }
        }
    }
}