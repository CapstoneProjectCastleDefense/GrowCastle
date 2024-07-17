namespace Runtime.Interfaces.Items
{
    using Runtime.Enums;

    public interface IEquipment : IItem, IHaveStats
    {
        void OnEquip(IHaveStatsModel target);
        void OnUnEquip(IHaveStatsModel target);
    }
}