namespace Installers
{
    using FunctionBase.Extensions;
    using FunctionBase.LocalDataManager;
    using Zenject;

    public class MainSceneInstaller : MonoInstaller<MainSceneInstaller>
    {
        public override void InstallBindings()
        {
            ReflectionExtension.GetAllDerivedTypes<ILocalData>().ForEach(type =>
            {
                var cachedData = this.Container.Resolve<HandleLocalDataService>().GetDataFromCached(type);
                this.Container.Rebind(type).FromInstance(cachedData).AsCached();
            });
            this.Container.Bind<AppService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();

        }
    }
}