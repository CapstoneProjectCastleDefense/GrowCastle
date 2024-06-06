namespace Models.LocalData
{
    using System;
    using GameFoundation.Scripts.Interfaces;
    using Models.LocalData.LocalDataController;

    public interface ILocalDataHaveController : ILocalData
    {
        public Type ControllerType { get; }
    }

    public interface ILocalDataHaveController<TController> : ILocalDataHaveController where TController : ILocalDataController
    {
        Type ILocalDataHaveController.ControllerType => typeof(TController);
    }
}