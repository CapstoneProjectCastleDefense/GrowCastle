namespace FunctionBase.Installers
{
    using FunctionBase.Extensions;
    using FunctionBase.LocalDataManager;
    using Zenject;

    public class GameFunctionBaseInstaller :  Installer<GameFunctionBaseInstaller>
    {
        public override void InstallBindings()
        {
            LocalDataInstaller.Install(this.Container);
            this.Container.Bind<AppService>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}