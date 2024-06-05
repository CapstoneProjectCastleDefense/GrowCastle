namespace Runtime.Elements.Entities.Archer
{
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;

    public abstract class BaseArcherModel : IElementModel, IHaveStats
    {
        public string                       Id              { get; set; }
        public string                       AddressableName { get; set; }
        public Dictionary<StatEnum, object> Stats           { get; set; }
    }
}