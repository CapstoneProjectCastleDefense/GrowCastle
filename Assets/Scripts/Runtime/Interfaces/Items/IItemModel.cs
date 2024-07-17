namespace Runtime.Interfaces.Items
{
    using Runtime.Elements.Base;
    using Runtime.Enums;

    public interface IItemModel : IHaveStatsModel, IIdentifier
    {
        ItemType      ItemType      { get; }
        EquipmentType EquipmentType { get; }
        int           Quantity      { get; set; }
    }
}