namespace Installers
{
    using FunctionBase.Extensions;
    using FunctionBase.LocalDataManager;
    using Zenject;

    public class MainSceneInstaller : MonoInstaller<MainSceneInstaller>
    {
        public override void InstallBindings()
        {
            this.BindLocalData();
            this.Container.Bind<AppService>().FromNewComponentOnNewGameObject().AsSingle().NonLazy();
        }

        public void BindLocalData()
        {
            ReflectionExtension.GetAllDerivedTypes<ILocalDataController>().ForEach(type =>
            {
                this.Container.Bind(type).AsCached();
            });

            ReflectionExtension.GetAllDerivedTypes<ILocalData>().ForEach(type =>
            {
                var cachedData = this.Container.Resolve<HandleLocalDataService>().GetDataFromCached(type);
                if ((cachedData as IDataHaveController)?.ControllerType is { } controllerType)
                {
                    this.Container.Bind(type).FromInstance(cachedData).WhenInjectedInto(controllerType);
                }
                else
                {
                    this.Container.Bind(type).FromInstance(cachedData).AsCached();
                }
            });
        }
    }
}