namespace Runtime.Elements.EntitySkills.ProjectileSkills
{
    using DG.Tweening;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;

    public class FireLightingBallSkill : BaseProjectileSkill<BaseProjectileSkillModel>
    {
        public FireLightingBallSkill(ProjectileManager projectileManager, IGameAssets gameAssets, ProjectileBlueprint projectileBlueprint, EffectManager effectManager)
            : base(projectileManager, gameAssets, projectileBlueprint, effectManager)
        {
        }
        public override string SkillId { get; set; } = EntitySkillName.FireLightingBall;
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
                    this.effectManager.AddEffectToTarget(targetableView.GetTargetablePresenter(), new InstantDamageTag() { Damage = this.Model.Damage });
                    projectile.GetView().transform.DOKill();
                    projectile.GetView().Recycle();
                    projectile.isFlyComplete = true;
                    this.RemoveProjectile(projectile);
                }
            }
        }
    }
}