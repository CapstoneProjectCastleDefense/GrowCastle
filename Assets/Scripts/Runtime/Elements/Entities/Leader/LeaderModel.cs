namespace Runtime.Elements.Entities.Leader
{
    using System;
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;
    using UnityEngine;

    public class LeaderModel : IElementModel, IHaveStats
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
        public Vector3                              StartPos        { get; set; }
        
        public LeaderModel(string id, string addressableName, Dictionary<StatEnum, (Type, object)> stats, Vector3 startPos)
        {
            this.Id              = id;
            this.AddressableName = addressableName;
            this.Stats           = stats;
            this.StartPos        = startPos;
        }
    }
}