namespace Runtime.Interfaces.Items
{
    using Runtime.Enums;

    public interface IEquipment : IItem
    {
        EquipmentType EquipmentType { get; }
    }
}