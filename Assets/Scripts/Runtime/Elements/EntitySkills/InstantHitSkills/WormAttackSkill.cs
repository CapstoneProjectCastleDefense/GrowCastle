namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Helpers;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.StaticValues;
    using UnityEngine;

    public class WormAttackSkill : InstantHitSkill<BasicSkillModel>
    {
        private readonly IGameAssets       gameAssets;
        private readonly ObjectPoolManager objectPoolManager;
        private readonly EffectManager     effectManager;

        public WormAttackSkill(SkillAttackBlueprint skillAttackBlueprint, IGameAssets gameAssets, ObjectPoolManager objectPoolManager, EffectManager effectManager) : base(skillAttackBlueprint)
        {
            this.gameAssets        = gameAssets;
            this.objectPoolManager = objectPoolManager;
            this.effectManager     = effectManager;
        }

        public override string SkillId { get; set; } = EntitySkillName.WormNormalAttack;

        protected override void InternalActivate()
        {
            var wormPrefab = this.gameAssets.LoadAssetAsync<GameObject>(this.VFXName).WaitForCompletion();
            var wormAttack = this.objectPoolManager.Spawn(wormPrefab, new(3f, -2, 0), Quaternion.identity);
            wormAttack.GetComponent<ObjectCollideEventHelper>().OnColliderTriggerEnter = this.OnWormCollide;
            wormAttack.GetComponent<ObjectAutoDestroy>().StartAutoDestroy(3);
        }

        private void OnWormCollide(GameObject collideObj, GameObject caster)
        {
            if (collideObj.layer != LayerMask.NameToLayer("Enemy")) return;
            var enemyPresenter = collideObj.GetComponentInParent<EnemyView>().Presenter;
            this.effectManager.AddEffectToTarget((ITargetable)enemyPresenter, new InstantDamageTag() { Damage = this.Damage });
            this.effectManager.AddEffectToTarget((ITargetable)enemyPresenter, new SlowTag() { Duration        = 0.5f, Timer = 0 });
        }
    }
}