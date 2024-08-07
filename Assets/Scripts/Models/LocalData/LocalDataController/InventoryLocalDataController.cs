namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Models.Blueprints;
    using Runtime.Enums;
    using Runtime.StaticValues;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class InventoryLocalDataController : ILocalDataController
    {
        private readonly InventoryLocalData      inventoryLocalData;
        private readonly ItemBlueprint           itemBlueprint;
        private readonly ItemRarityStatBlueprint itemRarityStatBlueprint;
        public InventoryLocalDataController(InventoryLocalData inventoryLocalData, ItemBlueprint itemBlueprint, ItemRarityStatBlueprint itemRarityStatBlueprint)
        {
            this.inventoryLocalData      = inventoryLocalData;
            this.itemBlueprint           = itemBlueprint;
            this.itemRarityStatBlueprint = itemRarityStatBlueprint;
        }
        public void InitData()
        {
            var resources = this.itemBlueprint.Values.Where(x => x.ItemType == ItemType.InventoryResource).ToList();
            foreach (var resource in resources)
            {
                var item = this.inventoryLocalData.Items.FirstOrDefault(x => x.BlueprintId == resource.Id);
                if (item == null)
                {
                    this.AddItem(resource.Id, 0, RarityEnum.Common, false, 0, 0, new());
                }
            }
        }

        public List<ItemData> GetItems(ItemType itemType) { return this.inventoryLocalData.Items.Where(x => this.itemBlueprint.GetDataById(x.BlueprintId).ItemType == itemType).ToList(); }
        public List<ItemData> GetAllItems()               { return this.inventoryLocalData.Items; }
        public ItemData       GetItem(string id)          { return this.inventoryLocalData.Items.FirstOrDefault(x => x.InventoryId == id); }
        public void AddItem(
            string blueprintId,
            int quantity,
            RarityEnum rarity,
            bool isEquipped,
            int level,
            int tier,
            Dictionary<StatEnum, (Type, object)> stats)
        {
            var sha256      = new SHA256Managed();
            var hash        = sha256.ComputeHash(Encoding.UTF8.GetBytes(DateTime.Now.ToString(CultureInfo.InvariantCulture) + DateTime.Now.Millisecond));
            var id          = BitConverter.ToString(hash).Replace("-", string.Empty);
            var isEquipment = this.itemBlueprint.GetDataById(blueprintId).ItemType == ItemType.Equipment;
            var itemData = new ItemData
            {
                InventoryId = id,
                BlueprintId = blueprintId,
                Quantity    = quantity,
                Rarity      = rarity,
                IsEquipped  = isEquipment && isEquipped,
                Level       = level,
                Tier        = tier,
                Stats       = stats
            };

            this.inventoryLocalData.Items.Add(itemData);
        }

        public void EquipItem(string itemId)
        {
            var item = this.inventoryLocalData.Items.FirstOrDefault(x => x.InventoryId == itemId);
            if (item == null) return;
            var blueprintData = this.itemBlueprint.GetDataById(item.BlueprintId);
            if (blueprintData.ItemType == ItemType.Equipment && !item.IsEquipped)
            {
                item.IsEquipped = true;
            }
        }

        public void UnEquipItem(string itemId)
        {
            var item = this.inventoryLocalData.Items.FirstOrDefault(x => x.InventoryId == itemId);
            if (item == null) return;
            var blueprintData = this.itemBlueprint.GetDataById(item.BlueprintId);
            if (blueprintData.ItemType == ItemType.Equipment && item.IsEquipped)
            {
                item.IsEquipped = false;
            }
        }

        public void RecycleItem(string itemId)
        {
            var item = this.inventoryLocalData.Items.FirstOrDefault(x => x.InventoryId == itemId);
            if (item == null) return;
            this.inventoryLocalData.Items.Remove(item);
            var fragmentCount = item.Rarity.GetFragments();
            item = this.inventoryLocalData.Items.FirstOrDefault(x => x.BlueprintId == MiscValue.ItemFragment);
            if (item == null)
            {
                this.AddItem(MiscValue.ItemFragment, fragmentCount, RarityEnum.Common, false, 0, 0, new());
            }
            else
            {
                item.Quantity += fragmentCount;
            }
        }
        public void RemoveItem(string itemId)
        {
            var item = this.inventoryLocalData.Items.FirstOrDefault(x => x.InventoryId == itemId);
            if (item == null) return;
            this.inventoryLocalData.Items.Remove(item);
        }

        public Dictionary<StatEnum, (Type, object)> GetStatOfItem(RarityEnum rarity)
        {
            var itemRarityRecord = this.itemRarityStatBlueprint.GetDataById(rarity);

            var attackRange        = itemRarityRecord.ItemRarityToStats[StatEnum.Attack];
            var attackSpeedRange   = itemRarityRecord.ItemRarityToStats[StatEnum.AttackSpeed];
            var skillCooldownRange = itemRarityRecord.ItemRarityToStats[StatEnum.ActiveSkillCooldown];

            var attackStat        = Random.Range(attackRange.StatRangeValue.minValue, attackRange.StatRangeValue.maxValue);
            var attackSpeedStat   = Random.Range(attackSpeedRange.StatRangeValue.minValue, attackSpeedRange.StatRangeValue.maxValue);
            var skillCooldownStat = Random.Range(skillCooldownRange.StatRangeValue.minValue, skillCooldownRange.StatRangeValue.maxValue);

            return new()
            {
                { StatEnum.Attack, (typeof(float),attackStat ) },
                { StatEnum.AttackSpeed, (typeof(float), attackSpeedStat) },
                { StatEnum.ActiveSkillCooldown, (typeof(float), skillCooldownStat) }
            };
        }
    }
}