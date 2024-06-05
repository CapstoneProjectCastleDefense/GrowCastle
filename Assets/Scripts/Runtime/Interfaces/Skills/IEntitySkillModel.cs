namespace Runtime.Interfaces.Skills
{
    using Runtime.Elements.Base;

    public interface IEntitySkillModel : IElementModel
    {

        string Description { get; }
        string Name        { get; }
        string IconPath    => this.AddressableName;
    }
}