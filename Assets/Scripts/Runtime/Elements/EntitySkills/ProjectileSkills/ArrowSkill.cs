namespace Runtime.Elements.EntitySkills
{
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Interfaces.Entities;
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
            EffectManager effectManager)
            : base(projectileManager, gameAssets, projectileBlueprint, abilitySystem, effectManager)
        {
        }

        public override void OnProjectileHit(Collider2D collider2D)
        {
            base.OnProjectileHit(collider2D);
            var objHit = collider2D.gameObject;
            if (objHit.layer == LayerMask.NameToLayer("Enemy"))
            {
                var targetableView = objHit.GetComponentInParent<ITargetableView>();
                if (targetableView != null)
                {
                    this.effectManager.Execute(targetableView.GetTargetablePresenter(), new InstantDamageTag() { Damage = 10 });
                }
            }
        }
    }

    public class ArrowSkillModel : BaseProjectileSkillModel
    {
    }
}