namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;
    using R3;

    public class ResourceLocalData : ILocalDataHaveController<ResourceLocalDataController>
    {
        public          ReactiveProperty<float>                           CurrentTargetExpToLevelUp { get; set; } = new(1000);
        public readonly Dictionary<ResourceType, ReactiveProperty<float>> Resource = new();
        public void Init()
        {
            this.Resource.Add(ResourceType.Gold,new ReactiveProperty<float>(1000));
            this.Resource.Add(ResourceType.Diamond,new ReactiveProperty<float>(100));
            this.Resource.Add(ResourceType.TalentPoint,new ReactiveProperty<float>(3));
            this.Resource.Add(ResourceType.Exp,new ReactiveProperty<float>(0));
        }
    }

    
    public enum ResourceType
    {
        Gold,
        Diamond,
        Ticket,
        TalentPoint,
        Exp,
        Item
    }
}