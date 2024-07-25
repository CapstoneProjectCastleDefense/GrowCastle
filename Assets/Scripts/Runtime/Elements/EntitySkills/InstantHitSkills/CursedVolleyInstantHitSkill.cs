namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using System.Collections.Generic;
    using DG.Tweening;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;

    public class CursedVolleyInstantHitSkill : InstantHitSkill<BasicSkillModel>
    {
        private readonly VFXService        vfxService;
        private readonly ObjectPoolManager objectPoolManager;
        private readonly IGameAssets       gameAssets;
        private readonly FindTargetSystem  findTargetSystem;
        private readonly EffectManager     effectManager;
        public CursedVolleyInstantHitSkill(
            SkillAttackBlueprint skillAttackBlueprint,
            VFXService vfxService,
            ObjectPoolManager objectPoolManager,
            IGameAssets gameAssets,
            FindTargetSystem findTargetSystem,
            EffectManager effectManager)
            : base(skillAttackBlueprint)
        {
            this.vfxService        = vfxService;
            this.objectPoolManager = objectPoolManager;
            this.gameAssets        = gameAssets;
            this.findTargetSystem  = findTargetSystem;
            this.effectManager     = effectManager;
        }
        public override string SkillId { get; set; } = EntitySkillName.CursedVolley;
        protected override void InternalActivate()
        {
            List<GameObject> arrows   = new();
            var          startPos = new Vector3(0f, 3, 0);
            var            range    = 0.5f;
            for (var i = 0; i < 3; i++)
            {
                var arrowPrefab = this.gameAssets.LoadAssetAsync<GameObject>(this.VFXName).WaitForCompletion();
                var arrow       = this.objectPoolManager.Spawn(arrowPrefab, startPos + new Vector3(range * i, 0), Quaternion.identity);
                arrows.Add(arrow);
            }

            var targets = this.findTargetSystem.GetRandomEnemies(3);
            if (targets == null) return;
            for (var i = 0; i < targets.Count; i++)
            {
                var index = i;
                arrows[index].transform.DOLookAt(targets[index].GetGameObject().transform.position, 0.1f,AxisConstraint.Y| AxisConstraint.W);
                arrows[index].transform.DOMove(targets[index].GetGameObject().transform.position, 0.5f).onComplete += () =>
                {
                    this.effectManager.AddEffectToTarget(targets[index], new InstantDamageTag() { Damage          = this.Damage });
                    this.effectManager.AddEffectToTarget(targets[index], new DeathEffectTag() { HpPercentRemainToTrigger = 100 });
                    arrows[index].Recycle();
                };
            }
        }
    }
}