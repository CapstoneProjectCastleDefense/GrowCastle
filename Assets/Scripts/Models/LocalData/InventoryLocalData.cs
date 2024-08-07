namespace Models.LocalData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.Scripts.Interfaces;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Interfaces.Items;
    using UnityEngine;

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
        public Dictionary<StatEnum, (Type, object)> BaseStats;
    }

    public static class ItemDataExtensions
    {
        public static ItemModel ToModel(this ItemData itemData, ItemBlueprint itemBlueprint) { return new(itemData, itemBlueprint); }
        public static float GetFinalStat(this ItemData itemData, StatEnum statEnum, out bool isExist)
        {
            if (itemData.BaseStats.TryGetValue(statEnum, out var stat))
            {
                var (type, value) = stat;
                isExist           = true;
                return (float)Convert.ChangeType(value, typeof(float)) * itemData.Rarity.GetMultiplier() * Mathf.Pow(1.1f, itemData.Level - 1) *
                       Mathf.Pow(1.5f, itemData.Tier);
            }
            else
            {
                isExist = false;
                return 0;
            }
        }

        public static float GetFinalStat(this ItemModel itemModel, StatEnum statEnum, out bool isExist)
        {
            if (itemModel.BaseStats.TryGetValue(statEnum, out var stat))
            {
                var (type, value) = stat;
                isExist           = true;
                return (float)Convert.ChangeType(value, typeof(float)) * itemModel.Rarity.GetMultiplier() * Mathf.Pow(1.1f, itemModel.Level - 1) *
                       Mathf.Pow(1.5f, itemModel.Tier);
            }
            else
            {
                isExist = false;
                return 0;
            }
        }
    }
}