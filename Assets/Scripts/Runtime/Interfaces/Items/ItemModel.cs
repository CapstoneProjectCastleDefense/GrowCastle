namespace Runtime.Interfaces.Items
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Models.Blueprints;
    using Models.LocalData;
    using Runtime.Elements.Base;
    using Runtime.Enums;

    public class ItemModel : IHaveStatsModel, IIdentifier
    {
        private readonly ItemBlueprint itemBlueprint;
        public ItemModel(ItemData itemData, ItemBlueprint itemBlueprint)
        {
            this.ItemData      = itemData;
            this.itemBlueprint = itemBlueprint;
        }

        public string Id { get => this.ItemData.BlueprintId; set { } }

        public string InventoryId => this.ItemData.InventoryId;

        public Dictionary<StatEnum, (Type, object)> Stats           { get => this.ItemData.Stats; set => this.ItemData.Stats = value; }
        public ItemType                             ItemType        => this.itemBlueprint.GetDataById(this.Id).ItemType;
        public int                                  Quantity        { get => this.ItemData.Quantity; protected set => this.ItemData.Quantity = value; }
        public RarityEnum                           Rarity          { get => this.ItemData.Rarity;   protected set => this.ItemData.Rarity = value; }
        public string                               AddressableName => this.itemBlueprint.GetDataById(this.Id).ImageAddress;
        public bool                                 IsEquipped      { get => this.ItemData.IsEquipped; protected set => this.ItemData.IsEquipped = value; }
        public int                                  Level           { get => this.ItemData.Level;      set => this.ItemData.Level = value; }
        public int                                  Tier            { get => this.ItemData.Tier;       set => this.ItemData.Tier = value; }
        public EquipmentType                        EquipmentType   => this.itemBlueprint.GetDataById(this.Id).EquipmentType;
        public ItemData                             ItemData        { get; protected set; }
    }
}