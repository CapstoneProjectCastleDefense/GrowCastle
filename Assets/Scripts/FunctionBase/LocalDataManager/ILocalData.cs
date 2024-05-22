namespace FunctionBase.LocalDataManager
{
    using System;

    public interface ILocalData
    {
        public void Init();
    }

    public interface ILocalData<TController> : ILocalData, IDataHaveController where TController : ILocalDataController
    {
        Type IDataHaveController.ControllerType => typeof(TController);
    }

    public interface IDataHaveController
    {
        public Type ControllerType { get;}
    }
}