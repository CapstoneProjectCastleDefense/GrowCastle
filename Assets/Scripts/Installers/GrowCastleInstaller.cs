namespace Installers
{
    using FunctionBase.Installers;
    using Zenject;

    public class GrowCastleInstaller : MonoInstaller<GrowCastleInstaller>
    {
        public override void InstallBindings()
        {
            GameFunctionBaseInstaller.Install(this.Container);
        }
    }
}