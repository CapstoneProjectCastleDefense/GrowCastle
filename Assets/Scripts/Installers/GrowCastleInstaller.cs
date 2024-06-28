namespace Installers
{
    using System;
    using GameFoundation.Scripts;
    using GameFoundation.Scripts.Interfaces;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.LocalData.LocalDataController;
    using Runtime.Managers;
    using Zenject;
    using Zenject.Internal;
    using ILocalDataHaveController = Models.LocalData.ILocalDataHaveController;

    public class GrowCastleInstaller : MonoInstaller<GrowCastleInstaller>
    {
        public override void InstallBindings()
        {
            GameFoundationInstaller.Install(this.Container);
            this.BindLocalData();
            this.BindAllController();
        }
        
        private void BindLocalData()
        {

            ReflectionUtils.GetAllDerivedTypes<ILocalData>().ForEach(type =>
            {
                var data = Activator.CreateInstance(type);
                if (type.DerivesFrom<ILocalDataHaveController>())
                {
                    if ((data as ILocalDataHaveController)?.ControllerType is { } controllerType)
                    {
                        this.Container.Bind(type).FromInstance(data).WhenInjectedInto(controllerType);
                    }
                    else
                    {
                        this.Container.Bind(type).FromInstance(data).AsCached();
                    }
                }
                else
                {
                    this.Container.Bind(type).FromInstance(data).AsCached();
                }
            });

            this.Container.Bind<UserDataManager>().AsCached();
        }
        
        private void BindAllController()
        {
            var listController = ReflectionUtils.GetAllDerivedTypes<ILocalDataController>();

            foreach (var controller in listController)
            {
                this.Container.BindInterfacesAndSelfTo(controller).AsCached();
            }
        }
    }
}