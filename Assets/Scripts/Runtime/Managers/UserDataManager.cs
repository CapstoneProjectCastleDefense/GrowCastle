namespace Runtime.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Interfaces;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.UserData;
    using Models.LocalData.LocalDataController;
    using Zenject;
    using ILocalDataHaveController = Models.LocalData.ILocalDataHaveController;

    public class UserDataManager
    {
        private readonly DiContainer                container;
        private readonly SignalBus                  signalBus;
        private readonly IHandleUserDataServices    handleUserDataService;
        private readonly List<ILocalDataController> localDataControllers;

        public UserDataManager(DiContainer container, SignalBus signalBus, IHandleUserDataServices handleUserDataService,List<ILocalDataController> localDataControllers)
        {
            this.container             = container;
            this.signalBus             = signalBus;
            this.handleUserDataService = handleUserDataService;
            this.localDataControllers  = localDataControllers;
        }

        public async UniTask LoadUserData()
        {
            var types     = ReflectionUtils.GetAllDerivedTypes<ILocalData>();
            var datas     = await this.handleUserDataService.Load(types.ToArray());
            var dataCache = (Dictionary<string, ILocalData>)typeof(BaseHandleUserDataServices).GetField("userDataCache", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(this.handleUserDataService);
            IterTools.Zip(types, datas).ForEach((type, data) =>
            {
                var boundData = (data as ILocalDataHaveController)?.ControllerType is { } controllerType
                    ? controllerType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                        .First(fieldInfo => fieldInfo.FieldType == type)
                        .GetValue(this.container.Resolve(controllerType))
                    : this.container.Resolve(type);
                data.CopyTo(boundData);
                dataCache[BaseHandleUserDataServices.KeyOf(type)] = (ILocalData)boundData;
            });
            this.signalBus.Fire<UserDataLoadedSignal>();
        }
    }
}