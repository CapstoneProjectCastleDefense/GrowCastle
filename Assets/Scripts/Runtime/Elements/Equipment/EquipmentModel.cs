namespace Runtime.Elements.Equipment
{
    using System;
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;

    public class EquipmentModel : IElementModel, IHaveStats
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
        public EquipmentType                         EquipmentType   { get; set; }
        
        public EquipmentModel(string id, string addressableName, Dictionary<StatEnum, (Type, object)> stats, EquipmentType equipmentType)
        {
            this.Id              = id;
            this.AddressableName = addressableName;
            this.Stats           = stats;
            this.EquipmentType   = equipmentType;
        }
    }
}