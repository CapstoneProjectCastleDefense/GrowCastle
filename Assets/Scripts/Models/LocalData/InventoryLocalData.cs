namespace Models.LocalData
{
    using System;
    using System.Collections.Generic;
    using GameFoundation.Scripts.Interfaces;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Interfaces.Items;

    public class InventoryLocalData : ILocalDataHaveController<InventoryLocalDataController>
    {
        public void Init() { }

        public List<ItemData> Items = new();
    }

    public class ItemData
    {
        public string                               InventoryId;
        public string                               BlueprintId;
        public int                                  Quantity;
        public int                                  Level;
        public int                                  Tier;
        public RarityEnum                           Rarity;
        public bool                                 IsEquipped;
        public Dictionary<StatEnum, (Type, object)> Stats;
    }

    public static class ItemDataExtensions
    {
        public static ItemModel ToModel(this ItemData itemData, ItemBlueprint itemBlueprint)
        {
            return new(itemData, itemBlueprint);
        }
    }
}