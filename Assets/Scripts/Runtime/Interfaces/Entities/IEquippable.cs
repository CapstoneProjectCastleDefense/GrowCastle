namespace Runtime.Interfaces.Entities
{
    using System.Collections.Generic;
    using Runtime.Enums;
    using Runtime.Interfaces.Items;

    public interface IEquippable
    {
        void Equip(string equipmentId);
        void UnEquip(string equipmentId);
        bool CanEquip();
        bool IsEquipped(string equipmentId);
    }
}