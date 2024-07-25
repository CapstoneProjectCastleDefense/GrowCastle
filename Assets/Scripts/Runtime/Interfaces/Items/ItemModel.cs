namespace Runtime.Interfaces.Items
{
    using System;
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Enums;

    public class ItemModel : IHaveStatsModel, IIdentifier
    {
        public ItemModel(string id, Dictionary<StatEnum, (Type, object)> stats, ItemType itemType, int quantity, RarityEnum rarity, string addressableName,
            bool isEquipped, int level, int tier, EquipmentType equipmentType)
        {
            this.Id              = id;
            this.Stats           = stats;
            this.ItemType        = itemType;
            this.Quantity        = quantity;
            this.Rarity          = rarity;
            this.AddressableName = addressableName;
            this.IsEquipped      = isEquipped;
            this.Level           = level;
            this.Tier            = tier;
            this.EquipmentType   = equipmentType;
        }
        public string                               Id              { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
        public ItemType                             ItemType        { get; protected set; }
        public int                                  Quantity        { get; protected set; }
        public RarityEnum                           Rarity          { get; protected set; }
        public string                               AddressableName { get; protected set; }
        public bool                                 IsEquipped      { get; protected set; }
        public int                                  Level           { get; set; }
        public int                                  Tier            { get; set; }
        public EquipmentType                        EquipmentType   { get; set; }
    }
}