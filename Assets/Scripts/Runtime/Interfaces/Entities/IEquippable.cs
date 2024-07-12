namespace Runtime.Interfaces.Entities
{
    using System.Collections.Generic;
    using Runtime.Enums;
    using Runtime.Interfaces.Items;

    public interface IEquippable
    {
        Dictionary<EquipmentType, IEquipment> Equipment { get; }
        void Equip(IEquipment equipment);
        void UnEquip(IEquipment equipment);
    }
}