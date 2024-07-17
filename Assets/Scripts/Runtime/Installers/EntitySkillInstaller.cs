namespace Runtime.Installers
{
    using GameFoundation.Scripts.Utilities.Extension;
    using Runtime.Interfaces.Abilities;
    using Runtime.Interfaces.Skills;
    using Runtime.Systems;
    using Zenject;

    public class EntitySkillInstaller : Installer<EntitySkillInstaller>
    {
        public override void InstallBindings()
        {
            foreach (var type in ReflectionUtils.GetAllDerivedTypes<IHeroSkill>())
            {
                if (!type.IsAbstract) this.Container.BindInterfacesAndSelfTo(type).AsCached().NonLazy();
            }
            
            this.Container.Bind<IAbility>().To(convention => convention.AllNonAbstractClasses()).WhenInjectedInto<AbilitySystem>();
        }
    }
}