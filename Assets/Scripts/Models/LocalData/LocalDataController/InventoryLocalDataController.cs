namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using Runtime.Enums;
    using Runtime.Interfaces.Items;

    public class InventoryLocalDataController : ILocalDataController
    {
        private readonly InventoryLocalData inventoryLocalData;
        public InventoryLocalDataController(InventoryLocalData inventoryLocalData) { this.inventoryLocalData = inventoryLocalData; }
        public void InitData() { }

        public List<IItemModel> GetItems(ItemType itemType) { return this.inventoryLocalData.Items.FindAll(item => item.ItemType == itemType); }
        public List<IItemModel> GetAllItems()               { return this.inventoryLocalData.Items; }
    }
}