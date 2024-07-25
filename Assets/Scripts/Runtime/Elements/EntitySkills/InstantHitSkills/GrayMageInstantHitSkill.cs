namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;

    public class GrayMageInstantHitSkill : InstantHitSkill<BasicSkillModel>
    {
        private readonly VFXService       vfxService;
        private readonly EnemyManager     enemyManager;
        private readonly FindTargetSystem findTargetSystem;
        private readonly EffectManager    effectManager;
        public GrayMageInstantHitSkill(
            SkillAttackBlueprint skillAttackBlueprint,
            VFXService vfxService,
            EnemyManager enemyManager,
            FindTargetSystem findTargetSystem,
            EffectManager effectManager)
            : base(skillAttackBlueprint)
        {
            this.vfxService       = vfxService;
            this.enemyManager     = enemyManager;
            this.findTargetSystem = findTargetSystem;
            this.effectManager    = effectManager;
        }
        public override string SkillId { get; set; } = EntitySkillName.GrayMageAttack;
        protected override void InternalActivate()
        {
            this.vfxService.SpawnVFX(this.VFXName, new Vector3(-1f, -1, 0), Quaternion.identity, scale: new Vector3(5, 5, 1));
            var targets = this.findTargetSystem.GetAllEnemyTarget();
            for (int i = 0; i < targets.Count; i++)
            {
                this.effectManager.AddEffectToTarget(targets[i], new InstantDamageTag() { Damage = this.Damage });
                this.effectManager.AddEffectToTarget(targets[i], new BleedTag() { Duration       = 3, Timer = 0, TimeDelay    = 0.2f });
                this.effectManager.AddEffectToTarget(targets[i], new SlowTag() { Duration        = 1, Timer = 0, InitialSpeed = targets[i].GetStats().GetStat<float>(StatEnum.MaxSpeed) });
            }
        }
    }
}