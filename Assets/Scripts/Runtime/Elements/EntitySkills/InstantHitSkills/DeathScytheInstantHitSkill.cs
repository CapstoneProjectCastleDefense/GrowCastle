namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using DG.Tweening;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Helpers;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.StaticValues;
    using UnityEngine;

    public class DeathScytheInstantHitSkill : InstantHitSkill<BasicSkillModel>
    {
        private readonly ObjectPoolManager objectPoolManager;
        private readonly IGameAssets       gameAssets;
        private readonly EffectManager     effectManager;
        public DeathScytheInstantHitSkill(SkillAttackBlueprint skillAttackBlueprint, ObjectPoolManager objectPoolManager, IGameAssets gameAssets, EffectManager effectManager)
            : base(skillAttackBlueprint)
        {
            this.objectPoolManager = objectPoolManager;
            this.gameAssets        = gameAssets;
            this.effectManager     = effectManager;
        }
        public override string SkillId { get; set; } = EntitySkillName.DeathScytheSkill;
        protected override void InternalActivate()
        {
            var scythePrefab = this.gameAssets.LoadAssetAsync<GameObject>(this.VFXName).WaitForCompletion();
            var scythe       = this.objectPoolManager.Spawn(scythePrefab, ((HeroView)((HeroPresenter)this.Model.Caster).GetView()).spawnProjectilePos.position, Quaternion.identity);
            scythe.transform.DOMove(new Vector3(-1, 1, 0), 0.5f);
            scythe.GetComponent<ObjectCollideEventHelper>().OnColliderTriggerEnter = this.OnScytheCollide;
            scythe.GetComponent<ObjectRotateItSelf>().StatRotate();
            scythe.GetComponent<ObjectAutoDestroy>().StartAutoDestroy(3);
        }

        private void OnScytheCollide(GameObject collideObj, GameObject caster)
        {
            if(collideObj.GetComponent<EnemyView>()==null) return;
            var enemyPresenter = collideObj.GetComponent<EnemyView>().Presenter;
            this.effectManager.AddEffectToTarget((ITargetable)enemyPresenter,new InstantDamageTag(){Damage = this.Damage});
        }
    }
}