namespace Runtime.Elements.Entities.Tower
{
    using System;
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using UnityEngine;

    public class TowerModel : ICombatant
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
        public Transform                            ParentView      { get; set; }
    }
}