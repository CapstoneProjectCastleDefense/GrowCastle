namespace Runtime.Interfaces.Items
{
    using Runtime.Enums;

    public interface IItem
    {
        ItemType ItemType { get; }
    }
}