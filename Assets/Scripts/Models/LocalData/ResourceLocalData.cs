namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;

    public class ResourceLocalData : ILocalDataHaveController<ResourceLocalDataController>
    {
        public Dictionary<ResourceType, float> resource = new();
        public void Init()
        {
            this.resource.Add(ResourceType.Gold,1000);
            this.resource.Add(ResourceType.Diamond,10);
        }
    }

    public enum ResourceType
    {
        Gold,
        Diamond,
        Ticket,
    }
}