namespace Models.LocalData.LocalDataController
{
    using System;
    using GameFoundation.Scripts.Interfaces;

    public interface ILocalDataHaveController
    {
        public Type ControllerType { get; }
    }

}