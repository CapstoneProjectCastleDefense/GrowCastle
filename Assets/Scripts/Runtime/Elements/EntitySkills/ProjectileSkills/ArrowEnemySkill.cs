namespace Runtime.Elements.EntitySkills.ProjectileSkills
{
    using System.Linq;
    using DG.Tweening;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Enums;
    using Runtime.Managers;
    using Runtime.StaticValues;
    using UnityEngine;

    public class ArrowEnemySkill : BaseProjectileSkill<BaseProjectileSkillModel>
    {
        private readonly   CastleManager castleManager;
        public override    string        SkillId     { get; set; } = EntitySkillName.ArrowEnemy;
        protected override string        targetLayer { get; set; } = "Castle";

        public ArrowEnemySkill(
            ProjectileManager   projectileManager,
            IGameAssets         gameAssets,
            ProjectileBlueprint projectileBlueprint,
            EffectManager       effectManager,
            CastleManager       castleManager
        )
            : base(projectileManager, gameAssets, projectileBlueprint, effectManager)
        {
            this.castleManager = castleManager;
        }

        protected override void OnProjectileHit(Collider2D collider2D, ProjectilePresenter projectile)
        {
        }

        protected override void OnFlyToTarget(ProjectilePresenter projectile)
        {
            var targetableView = this.castleManager.entities.First().CastleView;
            if (targetableView != null &&
                !targetableView.GetTargetablePresenter().IsDead)
            {
                targetableView.GetTargetablePresenter().OnGetHit(this.Model.Damage);
                projectile.GetView().transform.DOKill();
                projectile.GetView().Recycle();
                projectile.isFlyComplete = true;
                this.RemoveProjectile(projectile);
            }
        }
    }
}