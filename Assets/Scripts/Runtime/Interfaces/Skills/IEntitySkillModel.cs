namespace Runtime.Interfaces.Skills
{
    using Runtime.BasePoolAbleItem;

    public interface IEntitySkillModel : IPoolAbleItemModel
    {

        string Description { get; }
        string Name        { get; }
        string IconPath    => this.AddressableName;
    }
}