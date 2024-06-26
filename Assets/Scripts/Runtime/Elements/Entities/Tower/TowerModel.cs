﻿namespace Runtime.Elements.Entities.Tower
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;

    public class TowerModel : IElementModel, IHaveStats
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
        public Transform ParentView { get; set; }
    }
}