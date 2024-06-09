﻿namespace Runtime.Elements.Entities.Tower
{
    using System;
    using System.Collections.Generic;
    using Runtime.BasePoolAbleItem;
    using Runtime.Enums;
    using Runtime.Interfaces;

    public abstract class BaseTowerModel : IGameElementModel, IHaveStats
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
    }
}