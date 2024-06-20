﻿namespace Runtime.Elements.Entities.Leader
{
    using System;
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;

    public abstract class LeaderModel : IElementModel, IHaveStats
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
    }
}