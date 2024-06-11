namespace Installers
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.UIModule.Utilities;
    using GameFoundation.Scripts.Utilities.Extension;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Castles;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Elements.Entities.MapLevel;
    using Runtime.Elements.Entities.Slot;
    using Runtime.Managers;
    using Runtime.Managers.Base;
    using Runtime.Scenes;
    using Runtime.Signals;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.Systems;
    using Runtime.Systems.Waves;
    using Zenject;

    public class MainSceneInstaller : BaseSceneInstaller
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            this.DeclareSignals();
            
            this.Container.InitScreenManually<GameplayScreenPresenter>();

            this.BindAllSystem();
            this.BindAllManager();
            this.BindElement();
            GameStateMachineInstaller.Install(this.Container);
            
            WaveInstaller.Install(this.Container);
        }

        private void BindElement()
        {
            this.Container.BindFactory<CastleModel, CastlePresenter, BaseElementPresenter<CastleModel, CastleView, CastlePresenter>.Factory>().AsCached()
                .WhenInjectedInto<CastleManager>();
            this.Container.BindFactory<MapLevelModel, MapLevelPresenter, BaseElementPresenter<MapLevelModel, MapLevelView, MapLevelPresenter>.Factory>().AsCached()
                .WhenInjectedInto<MapLevelManager>();
            this.Container.BindFactory<SlotModel, SlotPresenter, BaseElementPresenter<SlotModel, SlotView, SlotPresenter>.Factory>().AsCached()
                .WhenInjectedInto<SlotManager>();
            this.Container.BindFactory<EnemyModel, EnemyPresenter, BaseElementPresenter<EnemyModel, EnemyView, EnemyPresenter>.Factory>().AsCached()
                .WhenInjectedInto<EnemyManager>();
        }

        private void BindAllManager()
        {
            foreach (var type in ReflectionUtils.GetAllDerivedTypes<IElementManager>())
            {
                if (!type.IsAbstract) this.Container.BindInterfacesAndSelfTo(type).AsCached().NonLazy();
            }
        }

        private void BindAllSystem()
        {
            foreach (var type in ReflectionUtils.GetAllDerivedTypes<IGameSystem>())
            {
                if (!type.IsAbstract) this.Container.BindInterfacesAndSelfTo(type).AsCached().NonLazy();
            }
        }

        private void DeclareSignals()
        {
            this.Container.DeclareSignal<TimeCooldownSignal>();
        }
    }
}