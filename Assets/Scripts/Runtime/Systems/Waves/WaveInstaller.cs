namespace Runtime.Systems.Waves
{
    using Runtime.Services;
    using Zenject;

    public class WaveInstaller : Installer<WaveInstaller>
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<EnemyGroupLoaderService>().AsSingle().NonLazy();
        }
    }
}