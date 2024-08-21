namespace Models.LocalData
{
    using System;
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;
    using R3;
    using Runtime.Enums;
    using Sirenix.Serialization;

    public class ResourceLocalData : ILocalDataHaveController<ResourceLocalDataController>
    {
        public                 ReactiveProperty<float>                           CurrentTargetExpToLevelUp { get; set; } = new(1000);
        public readonly        Dictionary<ResourceType, ReactiveProperty<float>> Resource = new();
        [OdinSerialize] public DateTime                                          LastDate { get; set; }
        public void Init()
        {
            this.Resource.Add(ResourceType.Gold,new(1000));
            this.Resource.Add(ResourceType.Diamond,new(100));
            this.Resource.Add(ResourceType.TalentPoint,new(0));
            this.Resource.Add(ResourceType.Exp,new(0));
            this.Resource.Add(ResourceType.Ticket,new(2));
            this.LastDate = DateTime.Now;
        }
    }
}