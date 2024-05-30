namespace FunctionBase
{
    using FunctionBase.AssetsManager;
    using FunctionBase.BlueprintManager;
    using FunctionBase.LocalDataManager;
    using Zenject;

    public class GameFunctionBaseInstaller :  Installer<GameFunctionBaseInstaller>
    {
        public override void InstallBindings()
        {
            SignalBusInstaller.Install(this.Container);
            LocalDataInstaller.Install(this.Container);
            BlueprintManagerInstaller.Install(this.Container);
            this.Container.Bind<GameAssetsManager>().AsSingle().NonLazy();
        }
    }
}