namespace Models.LocalData
{
    using System.Collections.Generic;
    using GameFoundation.Scripts.Interfaces;
    using Runtime.Interfaces.Items;

    public class InventoryLocalData : ILocalData
    {
        public void Init() {  }

        public List<IItem> Items = new();
    }
}