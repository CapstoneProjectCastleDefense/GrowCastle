namespace Runtime.Elements.Entities.Archer.Base
{
    using System;
    using System.Collections.Generic;
    using Runtime.BasePoolAbleItem;
    using Runtime.Enums;
    using Runtime.Interfaces;

    public abstract class BaseArcherModel : IGameElementModel, IHaveStats
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
    }
}