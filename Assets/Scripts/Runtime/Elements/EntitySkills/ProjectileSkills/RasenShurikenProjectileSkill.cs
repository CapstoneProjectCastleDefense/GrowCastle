namespace Runtime.Elements.EntitySkills.ProjectileSkills
{
    using System;
    using System.Collections.Generic;
    using DG.Tweening;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;

    public class RasenShurikenProjectileSkill : BaseProjectileSkill<BaseProjectileSkillModel>
    {
        private readonly FindTargetSystem findTargetSystem;
        public RasenShurikenProjectileSkill(ProjectileManager projectileManager, IGameAssets gameAssets, ProjectileBlueprint projectileBlueprint, EffectManager effectManager, FindTargetSystem findTargetSystem)
            : base(projectileManager, gameAssets, projectileBlueprint, effectManager)
        {
            this.findTargetSystem = findTargetSystem;
        }
        public override string SkillId { get; set; } = EntitySkillName.RasenShurikenSkill;

        public override void Activate(IEntitySkillModel baseSkillModel)
        {
            if (baseSkillModel is BaseProjectileSkillModel model)
            {
                this.Model = model;
            }

            var caster = (HeroPresenter)this.Model.Caster;
            this.Model.StartPoint = ((HeroView)caster.GetView()).spawnProjectilePos.position;
            this.Model.EndPoint   = caster.FindTarget().GetGameObject().transform.position;
            this.InternalActivate();
        }

        protected override void OnProjectileHit(Collider2D collider2D, ProjectilePresenter projectile)
        {
            base.OnProjectileHit(collider2D, projectile);
            var objHit = collider2D.gameObject;

            if (objHit.layer != LayerMask.NameToLayer("Enemy")) return;
            
            var targetAbleView = objHit.GetComponentInParent<ITargetableView>();
            if (targetAbleView == null || targetAbleView.GetTargetablePresenter().IsDead) return;
            this.effectManager.AddEffectToTarget(targetAbleView.GetTargetablePresenter(), new InstantDamageTag() { Damage = this.Model.Damage });
            projectile.GetView().transform.DOKill();
            projectile.GetView().Recycle();
            projectile.isFlyComplete = true;
                    
            var otherTarget = this.findTargetSystem.GetEnemiesInRange(this.Model.Caster, AttackPriorityEnum.Ground, targetAbleView.GetTargetablePresenter().GetGameObject().transform.position, 3);
            otherTarget.ForEach(target =>{this.effectManager.AddEffectToTarget(target,new InstantDamageTag(){Damage = this.Model.Damage});});
                    
            this.RemoveProjectile(projectile);
        }
    }
}