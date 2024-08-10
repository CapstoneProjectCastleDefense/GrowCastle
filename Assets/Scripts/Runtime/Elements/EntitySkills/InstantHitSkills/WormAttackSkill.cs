namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Helpers;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.StaticValues;
    using UnityEngine;

    public class WormAttackSkill : BaseProjectileSkill<BaseProjectileSkillModel>
    {
        private readonly IGameAssets       gameAssets;
        private readonly ObjectPoolManager objectPoolManager;
        private          GameObject        wormAttack;

        public WormAttackSkill(ProjectileManager projectileManager, IGameAssets gameAssets, ProjectileBlueprint projectileBlueprint, EffectManager effectManager, ObjectPoolManager objectPoolManager)
            : base(projectileManager, gameAssets, projectileBlueprint, effectManager)
        {
            this.gameAssets        = gameAssets;
            this.objectPoolManager = objectPoolManager;
        }

        public override string SkillId { get; set; } = EntitySkillName.WormNormalAttack;

        protected override void InternalActivate()
        {
            if (this.wormAttack != null)
            {
                this.wormAttack.GetComponent<ObjectAutoDestroy>().StartAutoDestroy(2);
                return;
            }
            var wormPrefab = this.gameAssets.LoadAssetAsync<GameObject>("WormAttack").WaitForCompletion();
            this.wormAttack = this.objectPoolManager.Spawn(wormPrefab, new(3f, -2, 0), Quaternion.identity);
            this.wormAttack.GetComponentInChildren<ObjectCollideEventHelper>().OnColliderTriggerEnter = this.OnWormCollide;
            this.wormAttack.GetComponent<ObjectAutoDestroy>().StartAutoDestroy(2);
        }

        private void OnWormCollide(GameObject collideObj, GameObject caster)
        {
            if (collideObj.layer != LayerMask.NameToLayer("Enemy")) return;
            var enemyPresenter = collideObj.GetComponentInParent<EnemyView>().Presenter;
            this.effectManager.AddEffectToTarget((ITargetable)enemyPresenter, new InstantDamageTag() { Damage = this.Model.Damage });
            this.effectManager.AddEffectToTarget((ITargetable)enemyPresenter, new SlowTag() { Duration        = 0.5f, Timer = 0 });
        }
    }
}