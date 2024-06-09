namespace Runtime.Elements.Entities.Hero
{
    using System;
    using System.Collections.Generic;
    using Runtime.BasePoolAbleItem;
    using Runtime.Enums;
    using Runtime.Interfaces;

    public abstract class BaseHeroModel : IGameElementModel, IHaveStats
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
    }
}