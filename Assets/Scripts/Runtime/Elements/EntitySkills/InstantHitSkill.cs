namespace Runtime.Elements.EntitySkills
{
    using Runtime.Enums;
    using Runtime.Interfaces.Skills;

    public class InstantHitSkill : BaseEntitySkillPresenter<InstantHitSkillModel>
    {
        public override EntitySkillType SkillType { get; set; } = EntitySkillType.InstantHit;
    }

    public class InstantHitSkillModel : BaseSkillModel
    {
    }
}