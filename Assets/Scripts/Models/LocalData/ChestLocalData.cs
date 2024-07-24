namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Newtonsoft.Json;
    using R3;

    public class ChestLocalData : ILocalDataHaveController<ChestLocalDataController>
    {
        public List<ChestData> ChestData = new();
        public bool   IsInit;

        public void Init()
        {
            
        }
    }
    public class ChestData
    {
        public ChestType   ChestType;
        [JsonIgnore]
        public ChestRecord ChestRecord;
    }
}