namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Runtime.Enums;
    using Runtime.Interfaces.Items;

    public class InventoryLocalDataController : ILocalDataController
    {
        private readonly InventoryLocalData inventoryLocalData;
        public InventoryLocalDataController(InventoryLocalData inventoryLocalData) { this.inventoryLocalData = inventoryLocalData; }
        public void InitData() { }

        public Dictionary<string, IItemModel> GetItems(ItemType itemType)
        {
            return this.inventoryLocalData.Items.Where(x => x.Value.ItemType == itemType)
                .ToDictionary(x => x.Key, y => y.Value);
        }
        public Dictionary<string, IItemModel> GetAllItems()      { return this.inventoryLocalData.Items; }
        public IItemModel                     GetItem(string id) { return this.inventoryLocalData.Items[id]; }
        public void AddItem(IItemModel item)
        {
            var sha256 = new System.Security.Cryptography.SHA256Managed();
            var hash   = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(DateTime.Now.ToString(CultureInfo.InvariantCulture) + DateTime.Now.Millisecond));
            var id     = BitConverter.ToString(hash).Replace("-", string.Empty);
            this.inventoryLocalData.Items.Add(id, item);
        }

        public void EquipItem(string itemId)
        {
            var item = this.inventoryLocalData.Items[itemId];
            if (item.ItemType == ItemType.Equipment && !item.IsEquipped)
            {
                item.IsEquipped = true;
            }
        }

        public void UnEquipItem(string itemId)
        {
            var item = this.inventoryLocalData.Items[itemId];
            if (item.ItemType == ItemType.Equipment && item.IsEquipped)
            {
                item.IsEquipped = false;
            }
        }
    }
}