namespace Runtime.Services
{
    using Zenject;

    public class ServiceInstaller : Installer<ToastController, ServiceInstaller>
    {
        private readonly ToastController toastController;

        public ServiceInstaller(ToastController toastController)
        {
            this.toastController = toastController;
        }
        
        public override void InstallBindings()
        {
            // Toast
            this.Container.Bind<ToastController>().FromComponentInNewPrefab(this.toastController).AsCached().NonLazy();
            
            // Gameplay
            this.Container.BindInterfacesAndSelfTo<TimeCoolDownService>().AsCached();
            
            // Element
            this.Container.Bind<HeroUpgradeService>().AsCached();
        }
    }
}