namespace Runtime.Elements.Entities.Hero
{
    using System;
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using UnityEngine;

    public class HeroModel : ICombatant
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
        public Transform                            ParentView      { get; set; }
        public List<string>                         Skills         { get; set; }
    }
}