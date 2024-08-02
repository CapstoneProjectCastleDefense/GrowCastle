namespace Runtime.Elements.EntitySkills.ProjectileSkills
{
    using System;
    using System.Collections.Generic;
    using DG.Tweening;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Helpers;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Elements.EntitySkills.InstantHitSkills;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;
    using Object = UnityEngine.Object;

    public class RasenShurikenInstantHitSkill : InstantHitSkill<BasicSkillModel>
    {
        private readonly ProjectileManager    projectileManager;
        private readonly IGameAssets          gameAssets;
        private readonly EffectManager        effectManager;
        private readonly FindTargetSystem     findTargetSystem;
        private readonly ObjectPoolManager    objectPoolManager;
        public RasenShurikenInstantHitSkill(
            ProjectileManager projectileManager,
            IGameAssets gameAssets,
            SkillAttackBlueprint skillAttackBlueprint,
            EffectManager effectManager,
            FindTargetSystem findTargetSystem,
            ObjectPoolManager objectPoolManager)
            : base(skillAttackBlueprint)
        {
            this.projectileManager    = projectileManager;
            this.gameAssets           = gameAssets;
            this.effectManager        = effectManager;
            this.findTargetSystem     = findTargetSystem;
            this.objectPoolManager    = objectPoolManager;
        }
        public override string SkillId { get; set; } = EntitySkillName.RasenShurikenSkill;
        protected override void InternalActivate()
        {
            var caster         = (HeroPresenter)this.Model.Caster;
            var startPoint     = ((HeroView)caster.GetView()).spawnProjectilePos.position;
            var endPoint       = caster.FindTarget().GetGameObject().transform.position;
            var shurikenPrefab = this.gameAssets.LoadAssetAsync<GameObject>(this.VFXName).WaitForCompletion();
            var shuriken       = this.objectPoolManager.Spawn(shurikenPrefab, startPoint, Quaternion.identity);
            shuriken.transform.DOMove(endPoint, 1f);
            shuriken.GetComponent<ObjectCollideEventHelper>().OnColliderTriggerEnter = this.OnProjectileHit;
        }

        private void OnProjectileHit(GameObject collideObj, GameObject caster)
        {
            var objHit = collideObj;

            if (objHit.layer != LayerMask.NameToLayer("Enemy")) return;

            var targetAbleView = objHit.GetComponentInParent<ITargetableView>();
            if (targetAbleView == null || targetAbleView.GetTargetablePresenter().IsDead) return;
            this.effectManager.AddEffectToTarget(targetAbleView.GetTargetablePresenter(), new InstantDamageTag() { Damage = this.Damage});
            caster.transform.DOKill();
            var otherTarget = this.findTargetSystem.GetEnemiesInRange(this.Model.Caster, AttackPriorityEnum.Ground, targetAbleView.GetTargetablePresenter().GetGameObject().transform.position, 3);
            otherTarget.ForEach(target => { this.effectManager.AddEffectToTarget(target, new InstantDamageTag() { Damage = this.Damage }); });
            Object.Destroy(caster);
        }
    }
}