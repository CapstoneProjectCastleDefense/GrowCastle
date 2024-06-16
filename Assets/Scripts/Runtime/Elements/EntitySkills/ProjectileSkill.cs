namespace Runtime.Elements.EntitySkills
{
    using Runtime.Enums;
    using Runtime.Interfaces.Skills;

    public class ProjectileSkill : BaseEntitySkillPresenter<InstantHitSkillModel>
    {
        public override EntitySkillType SkillType { get; set; } = EntitySkillType.Projectile;
    }
}