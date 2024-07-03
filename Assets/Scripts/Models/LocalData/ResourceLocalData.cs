namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;
    using R3;

    public class ResourceLocalData : ILocalDataHaveController<ResourceLocalDataController>
    {
        public Dictionary<ResourceType, ReactiveProperty<float>> resource = new();
        public void Init()
        {
            this.resource.Add(ResourceType.Gold,new(1000));
            this.resource.Add(ResourceType.Diamond,new(100));
        }
    }

    public enum ResourceType
    {
        Gold,
        Diamond,
        Ticket,
    }
}