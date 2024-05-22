namespace FunctionBase
{
    using FunctionBase.LocalDataManager;
    using Zenject;

    public class GameFunctionBaseInstaller :  Installer<GameFunctionBaseInstaller>
    {
        public override void InstallBindings()
        {
            LocalDataInstaller.Install(this.Container);
        }
    }
}