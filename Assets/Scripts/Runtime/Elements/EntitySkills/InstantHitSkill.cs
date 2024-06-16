namespace Runtime.Elements.EntitySkills
{
    using Runtime.Enums;
    using Runtime.Interfaces.Skills;

    public class InstantHitSkill : BaseEntitySkillPresenter<InstantHitSkillModel>
    {
        public override    EntitySkillType SkillType          { get; set; } = EntitySkillType.InstantHit;
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