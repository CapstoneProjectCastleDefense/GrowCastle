namespace FunctionBase.LocalDataManager
{
    using Zenject;

    public class LocalDataInstaller : Installer<LocalDataInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.Bind<HandleLocalDataService>().AsCached();
        }
    }
}