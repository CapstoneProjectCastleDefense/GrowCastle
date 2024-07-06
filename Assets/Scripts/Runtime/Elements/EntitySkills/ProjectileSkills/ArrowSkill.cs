namespace Runtime.Elements.EntitySkills
{
    using Runtime.Elements.Entities.Projectile;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;

    public class ArrowSkill : BaseProjectileSkill<ArrowSkillModel>
    {
        public override string SkillId { get; set; } = EntitySkillName.Arrow;
        
        public ArrowSkill(ProjectileManager projectileManager, AbilitySystem abilitySystem) : base(projectileManager, abilitySystem) { }

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