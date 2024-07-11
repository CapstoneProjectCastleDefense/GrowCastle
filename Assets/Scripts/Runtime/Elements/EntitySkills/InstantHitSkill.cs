namespace Runtime.Elements.EntitySkills
{
    using Runtime.Interfaces.Skills;

    public abstract class InstantHitSkill : BaseEntitySkillPresenter<InstantHitSkillModel>
    {
        protected override void            InternalActivate() { }
    }

    public class InstantHitSkillModel : IEntitySkillModel
    {
        public string Id              { get; set; }
        public string AddressableName { get; set; }
        public string Description     { get; }
        public string Name            { get; }
    }
}