namespace Runtime.Interfaces.Skills
{
    using Runtime.BasePoolAbleItem;
    using Runtime.Elements.Base;

    public interface ISkillModel : IElementModel
    {

        string Description { get; }
        string Name        { get; }
        string IconPath    => this.AddressableName;
    }
}