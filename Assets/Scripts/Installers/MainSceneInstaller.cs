namespace Installers
{
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.UIModule.Utilities;
    using GameFoundation.Scripts.Utilities.Extension;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Archer.Base;
    using Runtime.Elements.Entities.Castles;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Elements.Entities.Leader;
    using Runtime.Elements.Entities.MapLevel;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Elements.Entities.Slot;
    using Runtime.Elements.Entities.Summoner;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Managers.Base;
    using Runtime.Managers.Entity;
    using Runtime.Scenes;
    using Runtime.Services;
    using Runtime.Signals;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.Systems;
    using Runtime.Systems.Waves;
    using UnityEngine.EventSystems;
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
            this.BindAllSkill();
            this.BindService();
            this.BindMono();
            GameStateMachineInstaller.Install(this.Container);

            WaveInstaller.Install(this.Container);
            this.Container.Bind<EventSystem>().FromComponentInNewPrefabResource("EventSystem").AsSingle().NonLazy();
        }

        private void BindElement()
        {
            this.Container.BindFactory<CastleModel, CastlePresenter, CastlePresenter.Factory>().AsCached()
                .WhenInjectedInto<CastleManager>();
            this.Container.BindFactory<MapLevelModel, MapLevelPresenter, MapLevelPresenter.Factory>().AsCached()
                .WhenInjectedInto<MapLevelManager>();
            this.Container.BindFactory<SlotModel, SlotPresenter, SlotPresenter.Factory>().AsCached()
                .WhenInjectedInto<SlotManager>();
            this.Container.BindFactory<EnemyModel, EnemyPresenter, EnemyPresenter.Factory>().AsCached()
                .WhenInjectedInto<EnemyManager>();
            this.Container.BindFactory<ArcherModel, ArcherPresenter, ArcherPresenter.Factory>().AsCached()
                .WhenInjectedInto<ArcherManager>();
            this.Container.BindFactory<HeroModel, HeroPresenter, HeroPresenter.Factory>().AsCached()
                .WhenInjectedInto<HeroManager>();
            this.Container.BindFactory<ProjectileModel, ProjectilePresenter, ProjectilePresenter.Factory>().AsCached()
                .WhenInjectedInto<ProjectileManager>();
            this.Container.BindFactory<LeaderModel, LeaderPresenter, LeaderPresenter.Factory>().AsCached()
                .WhenInjectedInto<LeaderManager>();
            this.Container.BindFactory<SummonerModel, SummonerPresenter, SummonerPresenter.Factory>().AsCached()
                .WhenInjectedInto<SummonerManager>();
        }

        private void BindAllManager()
        {
            foreach (var type in ReflectionUtils.GetAllDerivedTypes<IElementManager>())
            {
                if (!type.IsAbstract)
                {
                    this.Container.BindInterfacesAndSelfTo(type).AsCached().NonLazy();
                }
            }
        }

        private void BindAllSystem()
        {
            this.Container.Bind<GetCustomPresenterSystem>().AsSingle().NonLazy();
            foreach (var type in ReflectionUtils.GetAllDerivedTypes<IGameSystem>())
            {
                if (!type.IsAbstract) this.Container.BindInterfacesAndSelfTo(type).AsCached().NonLazy();
            }
        }

        private void BindAllSkill()
        {
            foreach (var type in ReflectionUtils.GetAllDerivedTypes<IEntitySkillPresenter>())
            {
                if (!type.IsAbstract) this.Container.BindInterfacesAndSelfTo(type).AsCached().NonLazy();
            }
        }

        private void BindService()
        {
            this.Container.BindInterfacesAndSelfTo<TimeCoolDownService>().AsCached();
        }

        private void BindMono()
        {
            this.Container.Bind<SlotConfig>().FromComponentInHierarchy().AsCached().NonLazy();
        }

        private void DeclareSignals()
        {
            this.Container.DeclareSignal<TimeCooldownSignal>();
            this.Container.DeclareSignal<UpdateCastleStatSignal>();
        }
    }
}