namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Models.Blueprints;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.StaticValues;

    public class InventoryLocalDataController : ILocalDataController
    {
        private readonly InventoryLocalData inventoryLocalData;
        private readonly ItemBlueprint      itemBlueprint;
        public InventoryLocalDataController(InventoryLocalData inventoryLocalData, ItemBlueprint itemBlueprint)
        {
            this.inventoryLocalData = inventoryLocalData;
            this.itemBlueprint      = itemBlueprint;
        }
        public void InitData() { }

        public List<ItemData> GetItems(ItemType itemType)
        {
            return this.inventoryLocalData.Items.Where(x => this.itemBlueprint.GetDataById(x.BlueprintId).ItemType == itemType).ToList();
        }
        public List<ItemData> GetAllItems()      { return this.inventoryLocalData.Items; }
        public ItemData       GetItem(string id) { return this.inventoryLocalData.Items.FirstOrDefault(x => x.InventoryId == id); }
        public void AddItem(string blueprintId, int quantity, RarityEnum rarity, bool isEquipped, int level, int tier, Dictionary<StatEnum, (Type, object)> stats)
        {
            var sha256      = new System.Security.Cryptography.SHA256Managed();
            var hash        = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString(CultureInfo.InvariantCulture) + DateTime.Now.Millisecond));
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
            item = this.inventoryLocalData.Items.FirstOrDefault(x => x.BlueprintId == ResourceValue.ItemFragment);
            if (item == null)
            {
                this.AddItem(ResourceValue.ItemFragment, fragmentCount, RarityEnum.Common, false, 0, 0, new());
            }
            else
            {
                item.Quantity += fragmentCount;
            }
        }
    }
}