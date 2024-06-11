namespace Runtime.Systems.Waves
{
    using Zenject;

    public class WaveInstaller : Installer<WaveInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<WaveLoader>().AsSingle().NonLazy();
        }
    }
}