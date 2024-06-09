namespace Runtime.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Interfaces;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.UserData;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Zenject;

    public class UserDataManager
    {
        private readonly DiContainer             container;
        private readonly SignalBus               signalBus;
        private readonly IHandleUserDataServices handleUserDataService;

        public UserDataManager(DiContainer container, SignalBus signalBus, IHandleUserDataServices handleUserDataService)
        {
            this.container             = container;
            this.signalBus             = signalBus;
            this.handleUserDataService = handleUserDataService;
        }

        public async UniTask LoadUserData()
        {
            var types     = ReflectionUtils.GetAllDerivedTypes<ILocalData>();
            var datas     = await this.handleUserDataService.Load(types.ToArray());
            var dataCache = (Dictionary<string, ILocalData>)typeof(BaseHandleUserDataServices).GetField("userDataCache", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(this.handleUserDataService);
            IterTools.Zip(types, datas).ForEach((type, data) =>
            {
                var boundData = this.container.Resolve(type);

                data.CopyTo(boundData);
                dataCache[BaseHandleUserDataServices.KeyOf(type)] = (ILocalData)boundData;
                if (!typeof(ILocalDataHaveController).IsAssignableFrom(type)) return;
                var controllerType = ((ILocalDataHaveController)data).ControllerType;
                this.container.BindInterfacesAndSelfTo(controllerType).AsCached();
                this.container.Rebind(type).AsCached().WhenInjectedInto(controllerType);

            });
            this.signalBus.Fire<UserDataLoadedSignal>();
        }
    }
}