namespace Models.LocalData.LocalDataController
{
    using System;
    using GameFoundation.Scripts.Interfaces;

    public interface ILocalDataHaveController : ILocalData
    {
        public Type ControllerType { get; }
    }

    public interface ILocalDataHaveController<TController> : ILocalDataHaveController where TController : ILocalDataController
    {
        Type ILocalDataHaveController.ControllerType => typeof(TController);
    }
}