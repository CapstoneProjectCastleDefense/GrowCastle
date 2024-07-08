namespace Runtime.Elements.EntitySkills
{
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Managers;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;

    public class ArrowSkill : BaseProjectileSkill<ArrowSkillModel>
    {
        public override string SkillId { get; set; } = EntitySkillName.Arrow;
        public ArrowSkill(ProjectileManager projectileManager, 
                          IGameAssets gameAssets, 
                          ProjectileBlueprint projectileBlueprint, 
                          AbilitySystem abilitySystem, 
                          AffectManager affectManager)
            : base(projectileManager, gameAssets, projectileBlueprint, abilitySystem, affectManager) { }

        public override void OnProjectileHit(Collider2D collider2D)
        {
            base.OnProjectileHit(collider2D);
            if (collider2D.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
            }
        }
    }

    public class ArrowSkillModel : BaseProjectileSkillModel
    {
    }
}