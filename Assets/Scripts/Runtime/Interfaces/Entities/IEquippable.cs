namespace Runtime.Interfaces.Entities
{
    using Runtime.Interfaces.Items;

    public interface IEquippable
    {
        void Equip(IEquipment equipment);
        void UnEquip(IEquipment equipment);
    }
}