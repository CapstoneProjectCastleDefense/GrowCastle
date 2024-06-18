namespace Runtime.Interfaces.Skills
{
    using Runtime.Elements.Base;

    public interface IEntitySkillModel : IElementModel
    {
        string Description { get; }
        string Name        { get; }
        string IconPath    => this.AddressableName;
    }

    public class BasicSkillModel : IEntitySkillModel
    {
        public string Id              { get; set; }
        public int    Level           { get; set; }
        public string AddressableName { get; set; }
        public string Description     { get; }
        public string Name            { get; }
    }
}