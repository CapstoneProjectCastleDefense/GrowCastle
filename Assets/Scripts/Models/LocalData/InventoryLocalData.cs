namespace Models.LocalData
{
    using System.Collections.Generic;
    using GameFoundation.Scripts.Interfaces;
    using Models.LocalData.LocalDataController;
    using Runtime.Interfaces.Items;

    public class InventoryLocalData : ILocalDataHaveController<InventoryLocalDataController>
    {
        public void Init() {  }

        public List<IItem> Items = new();
    }
}