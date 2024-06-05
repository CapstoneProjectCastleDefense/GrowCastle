﻿namespace Runtime.Elements.Entities.Leader
{
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;

    public abstract class BaseLeaderModel : IElementModel, IHaveStats
    {
        public string                       Id              { get; set; }
        public string                       AddressableName { get; set; }
        public Dictionary<StatEnum, object> Stats           { get; set; }
    }
}