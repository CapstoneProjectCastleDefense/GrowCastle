namespace Installers
{
    using System;
    using GameFoundation.Scripts;
    using GameFoundation.Scripts.Interfaces;
    using GameFoundation.Scripts.Utilities.Extension;
    using Runtime.Managers;
    using Zenject;

    public class GrowCastleInstaller : MonoInstaller<GrowCastleInstaller>
    {
        public override void InstallBindings()
        {
            GameFoundationInstaller.Install(this.Container);
            this.BindLocalData();
        }
        
        private void BindLocalData()
        {
            ReflectionUtils.GetAllDerivedTypes<ILocalData>().ForEach(type =>
            {
                var data = Activator.CreateInstance(type);
                this.Container.Bind(type).FromInstance(data).AsCached();
            });

            this.Container.Bind<UserDataManager>().AsCached();
        }
    }
}