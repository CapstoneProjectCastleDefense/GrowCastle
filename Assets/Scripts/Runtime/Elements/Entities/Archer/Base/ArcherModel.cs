namespace Runtime.Elements.Entities.Archer.Base
{
    using System;
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;
    using UnityEngine;

    public class ArcherModel : IElementModel, IHaveStats
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
        public int                                  Level           { get; set; }
        public int                                  Index           { get; set; }
        public Transform                            ParentView   { get; set; }
    }
}