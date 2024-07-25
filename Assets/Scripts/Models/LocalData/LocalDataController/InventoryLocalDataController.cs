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
        public void AddItem(ItemModel item)
        {
            var sha256      = new System.Security.Cryptography.SHA256Managed();
            var hash        = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString(CultureInfo.InvariantCulture) + DateTime.Now.Millisecond));
            var id          = BitConverter.ToString(hash).Replace("-", string.Empty);
            var isEquipment = this.itemBlueprint.GetDataById(item.Id).ItemType == ItemType.Equipment;
            var itemData = new ItemData
            {
                InventoryId = id,
                BlueprintId = item.Id,
                Quantity    = item.Quantity,
                Rarity      = item.Rarity,
                IsEquipped  = isEquipment && item.IsEquipped,
                Level       = item.Level,
                Tier        = item.Tier,
                Stats       = item.Stats
            };

            this.inventoryLocalData.Items.Add(itemData);
        }

        public void EquipItem(string itemId)
        {
            var item = this.inventoryLocalData.Items.FirstOrDefault(x => x.InventoryId == itemId);
            if(item == null) return;
            var blueprintData = this.itemBlueprint.GetDataById(item.BlueprintId);
            if (blueprintData.ItemType == ItemType.Equipment && !item.IsEquipped)
            {
                item.IsEquipped = true;
            }
        }

        public void UnEquipItem(string itemId)
        {
            var item = this.inventoryLocalData.Items.FirstOrDefault(x => x.InventoryId == itemId);
            if(item == null) return;
            var blueprintData = this.itemBlueprint.GetDataById(item.BlueprintId);
            if (blueprintData.ItemType == ItemType.Equipment && item.IsEquipped)
            {
                item.IsEquipped = false;
            }
        }
    }
}