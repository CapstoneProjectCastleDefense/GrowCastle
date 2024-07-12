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

        public List<IItem> GetItems(ItemType itemType) { return new List<IItem>(); }
    }
}