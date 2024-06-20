namespace Runtime.Installers
{
    using GameFoundation.Scripts.Utilities.Extension;
    using Runtime.Elements.EntitySkillEffect;
    using Runtime.Executors;
    using Runtime.Interfaces.Skills;
    using Zenject;

    public class EntitySkillInstaller : Installer<EntitySkillInstaller>
    {
        public override void InstallBindings()
        {
            foreach (var type in ReflectionUtils.GetAllDerivedTypes<IEntitySkillPresenter>())
            {
                if (!type.IsAbstract) this.Container.BindInterfacesAndSelfTo(type).AsCached().NonLazy();
            }

            this.Container.BindInterfacesAndSelfTo<EntitySkillEffectExecutor>().AsSingle();
            this.Container.Bind<IEntitySkillEffect>().To(convention => convention.AllNonAbstractClasses()).WhenInjectedInto<EntitySkillEffectExecutor>();
        }
    }
}