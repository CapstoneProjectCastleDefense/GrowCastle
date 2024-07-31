namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using Models.Blueprints;
    using Runtime.Interfaces.Skills;
    using Runtime.StaticValues;

    public class DeathScytheInstantHitSkill : InstantHitSkill<BasicSkillModel>
    {
        public DeathScytheInstantHitSkill(SkillAttackBlueprint skillAttackBlueprint)
            : base(skillAttackBlueprint)
        {
        }
        public override string SkillId { get; set; } = EntitySkillName.DeathScytheSkill;
        protected override void InternalActivate()
        {
            
        }
    }
}