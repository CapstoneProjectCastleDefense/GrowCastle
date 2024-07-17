namespace Runtime.Elements.Equipment
{
    using System;
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;
    using Runtime.Interfaces.Items;

    public class EquipmentModel : IItemModel, IElementModel
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
        public ItemType                             ItemType        { get; }
        public EquipmentType                        EquipmentType   { get; set; }
        public int                                  Quantity        { get; set; }

        public EquipmentModel(string id, string addressableName, Dictionary<StatEnum, (Type, object)> stats, EquipmentType equipmentType, ItemType itemType)
        {
            this.Id              = id;
            this.AddressableName = addressableName;
            this.Stats           = stats;
            this.EquipmentType   = equipmentType;
            this.ItemType        = itemType;
        }
    }
}