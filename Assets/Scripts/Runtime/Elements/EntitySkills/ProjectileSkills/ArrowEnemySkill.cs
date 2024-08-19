namespace Runtime.Elements.EntitySkills.ProjectileSkills
{
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Managers;
    using Runtime.StaticValues;

    public class ArrowEnemySkill : BaseProjectileSkill<BaseProjectileSkillModel>
    {
        public override string SkillId { get; set; } = EntitySkillName.ArrowEnemy;

        public ArrowEnemySkill(
            ProjectileManager   projectileManager,
            IGameAssets         gameAssets,
            ProjectileBlueprint projectileBlueprint,
            EffectManager       effectManager
        )
            : base(projectileManager, gameAssets, projectileBlueprint, effectManager)
        {
        }
    }
}