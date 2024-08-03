namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;

    public class GoldenThunderInstantHit : InstantHitSkill<BasicSkillModel>
    {
        private readonly VFXService       vfxService;
        private readonly FindTargetSystem findTargetSystem;
        private readonly EffectManager    effectManager;

        public GoldenThunderInstantHit(SkillAttackBlueprint skillAttackBlueprint, IGameAssets gameAssets, VFXService vfxService, FindTargetSystem findTargetSystem, EffectManager effectManager) : base(skillAttackBlueprint)
        {
            this.vfxService       = vfxService;
            this.findTargetSystem = findTargetSystem;
            this.effectManager    = effectManager;
        }

        public override string SkillId { get; set; } = EntitySkillName.GoldenThunderSkill;

        protected override void InternalActivate()
        {
            var startPos = new Vector3(-2f, -2, 0);
            for (var i = 0; i < 2; i++)
            {
                var pos = startPos + new Vector3(i * 5, 0, 0);
                this.vfxService.SpawnVFX(this.VFXName, pos, Quaternion.identity, scale: new Vector3(3, 3, 1));
                var targets = this.findTargetSystem.GetEnemiesInRange(this.Model.Caster, AttackPriorityEnum.Ground, pos, 8);
                foreach (var t in targets)
                {
                    this.effectManager.AddEffectToTarget(t, new InstantDamageTag() { Damage = this.Damage });
                }
            }
        }
    }
}