namespace Installers
{
    using GameFoundation.Scripts;
    using Zenject;

    public class GrowCastleInstaller : MonoInstaller<GrowCastleInstaller>
    {
        public override void InstallBindings()
        {
            GameFoundationInstaller.Install(this.Container);
        }
    }
}