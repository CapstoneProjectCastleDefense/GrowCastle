namespace Installers
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.UIModule.Utilities;
    using GameFoundation.Scripts.Utilities.Extension;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Castles;
    using Runtime.Elements.Entities.MapLevel;
    using Runtime.Managers;
    using Runtime.Managers.Base;
    using Runtime.Scenes;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.Systems;

    public class MainSceneInstaller : BaseSceneInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            this.Container.InitScreenManually<GameplayScreenPresenter>();

            this.BindAllSystem();
            this.BindAllManager();
            this.BindElement();
            GameStateMachineInstaller.Install(this.Container);

        }

        private void BindElement()
        {
            this.Container.BindFactory<CastleModel, CastlePresenter, CastlePresenter.Factory>().AsCached()
                .WhenInjectedInto<CastleManager>();
            this.Container.BindFactory<MapLevelModel, MapLevelPresenter, MapLevelPresenter.Factory>().AsCached()
                .WhenInjectedInto<MapLevelManager>();
        }

        private void BindAllManager()
        {
            foreach (var type in ReflectionUtils.GetAllDerivedTypes<IElementManager>())
            {
                if(!type.IsAbstract) this.Container.BindInterfacesAndSelfTo(type).AsCached().NonLazy();
            }
        }

        private void BindAllSystem()
        {
            foreach (var type in ReflectionUtils.GetAllDerivedTypes<IGameSystem>())
            {
                if(!type.IsAbstract) this.Container.BindInterfacesAndSelfTo(type).AsCached().NonLazy();
            }
        }
    }
}