namespace Runtime.Elements.Entities.Hero
{
    using System;
    using System.Collections.Generic;
    using Models.Tags;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;
    using UnityEngine;

    public class HeroModel : ICombatant
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> BaseStats           { get; set; }
        public Transform                            ParentView      { get; set; }
    }
}