namespace Runtime.Elements.Entities.Hero
{
    using System.Collections.Generic;
    using Runtime.BasePoolAbleItem;
    using Runtime.Enums;
    using Runtime.Interfaces;

    public abstract class BaseHeroModel : IPoolAbleItemModel, IHaveStats
    {
        public string                       Id              { get; set; }
        public string                       AddressableName { get; set; }
        public Dictionary<StatEnum, object> Stats           { get; set; }
    }
}