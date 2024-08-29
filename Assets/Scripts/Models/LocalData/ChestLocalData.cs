namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Newtonsoft.Json;
    using R3;
    using Runtime.Enums;

    public class ChestLocalData : ILocalDataHaveController<ChestLocalDataController>
    {
        public List<ChestData> ChestData;
        public bool            IsInit;

        public void Init() { }
    }

    public class ChestData
    {
        public              ResourceType ChestType;
        [JsonIgnore] public ChestRecord  ChestRecord;
    }
}