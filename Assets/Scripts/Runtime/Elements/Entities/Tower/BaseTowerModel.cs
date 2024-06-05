namespace Runtime.Elements.Entities.Tower
{
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;

    public abstract class BaseTowerModel : IElementModel, IHaveStats
    {
        public string                       Id              { get; set; }
        public string                       AddressableName { get; set; }
        public Dictionary<StatEnum, object> Stats           { get; set; }
    }
}