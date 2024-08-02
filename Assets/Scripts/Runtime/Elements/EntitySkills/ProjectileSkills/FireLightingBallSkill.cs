namespace Runtime.Elements.EntitySkills.ProjectileSkills
{
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Managers;
    using Runtime.StaticValues;

    public class FireLightingBallSkill : BaseProjectileSkill<BaseProjectileSkillModel>
    {
        public FireLightingBallSkill(ProjectileManager projectileManager, IGameAssets gameAssets, ProjectileBlueprint projectileBlueprint, EffectManager effectManager)
            : base(projectileManager, gameAssets, projectileBlueprint, effectManager)
        {
        }
        public override string SkillId { get; set; } = EntitySkillName.FireLightingBall;
    }
}