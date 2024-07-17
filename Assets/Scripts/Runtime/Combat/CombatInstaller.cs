namespace Runtime.Combat
{
    using Runtime.Combat.CombatActions;
    using Runtime.Combat.CombatActions.Actions;
    using Zenject;

    public class CombatInstaller : Installer<CombatInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.Bind<CombatActionExecutor>().AsCached(); 
            this.Container.Bind<ICombatAction>().To(convention => convention.AllNonAbstractClasses().DerivingFrom<ICombatAction>()).WhenInjectedInto<CombatActionExecutor>().NonLazy();
        }
    }
}