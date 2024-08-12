namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;

    public class WizardAttackSkill : InstantHitSkill<BasicSkillModel>
    {
        private readonly VFXService       vfxService;
        private readonly EnemyManager     enemyManager;
        private readonly FindTargetSystem findTargetSystem;
        private readonly EffectManager    effectManager;
        public WizardAttackSkill(
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
        public override string SkillId { get; set; } = EntitySkillName.WizardAttackSkill;
        protected override void InternalActivate()
        {
            this.vfxService?.SpawnVFX(this.VFXName, new Vector3(3f, -2, 0), Quaternion.identity, scale: new Vector3(4, 4, 1));
            var targets = this.findTargetSystem.GetEnemiesInRange(this.Model.Caster, AttackPriorityEnum.Ground, new Vector3(3f, -2, 0), 7);
            for (int i = 0; i < targets.Count; i++)
            {
                this.effectManager.AddEffectToTarget(targets[i], new InstantDamageTag() { Damage = this.Damage * 3 });
                this.effectManager.AddEffectToTarget(targets[i], new BleedTag() { Duration       = 2, Timer = 0, TimeDelay = 0.1f });
                this.effectManager.AddEffectToTarget(targets[i], new SlowTag() { Duration        = 1, Timer = 0 });
            }
        }
    }
}