namespace Runtime.Elements.EntitySkills.ProjectileSkills
{
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;
    using Zenject;

    public class FireLightingBallSkill : BaseProjectileSkill<BaseProjectileHeroSkillModel>
    {
        public FireLightingBallSkill(SignalBus signalBus,
                                     FindTargetSystem findTargetSystem,
                                     EffectManager effectManager, 
                                     VFXService vfxService, 
                                     ProjectileManager projectileManager,
                                     ProjectileBlueprint projectileBlueprint) 
            : base(signalBus, findTargetSystem, effectManager, vfxService, projectileManager, projectileBlueprint)
        {
        }

        public override string SkillId                                   { get; set; } = EntitySkillName.FireLightingBall;
        public override void   Activate(ICombatantPresenter caster)      {  }
        public override void   Deactivate(ICombatantPresenter combatant) {  }

        protected override void OnProjectileHit(Collider2D collider2D, ProjectilePresenter projectile)
        {
            base.OnProjectileHit(collider2D, projectile);
            var objHit = collider2D.gameObject;
            //todo: check target layer mask from model instead of static input
            if (objHit.layer == LayerMask.NameToLayer("Enemy"))
            {
                var targetableView = objHit.GetComponentInParent<ITargetableView>();
                if (targetableView != null &&
                    !targetableView.GetTargetablePresenter().IsDead)
                {
                    this.effectManager.Execute(targetableView.GetTargetablePresenter(), new InstantDamageTag() { Damage = FireLightingBallSkillData.Damage });
                    projectile.GetView().transform.DOKill();
                    projectile.GetView().Recycle();
                    projectile.isFlyComplete = true;
                    this.RemoveProjectile(projectile);
                }
            }
        }
    }

    public static class FireLightingBallSkillData
    {
        public const float Damage = 10;
    }
}