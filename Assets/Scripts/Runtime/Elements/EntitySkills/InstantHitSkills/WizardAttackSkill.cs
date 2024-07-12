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

    public class WizardAttackSkill : InstantHitSkill<BasicSkillModel>
    {
        private readonly VFXService       vfxService;
        private readonly EnemyManager     enemyManager;
        private readonly FindTargetSystem findTargetSystem;
        private readonly AbilitySystem    abilitySystem;
        private readonly EffectManager    effectManager;
        public WizardAttackSkill(
            SkillAttackBlueprint skillAttackBlueprint,
            VFXService vfxService,
            EnemyManager enemyManager,
            FindTargetSystem findTargetSystem,
            AbilitySystem abilitySystem,
            EffectManager effectManager)
            : base(skillAttackBlueprint)
        {
            this.vfxService       = vfxService;
            this.enemyManager     = enemyManager;
            this.findTargetSystem = findTargetSystem;
            this.abilitySystem    = abilitySystem;
            this.effectManager    = effectManager;
        }
        public override string SkillId { get; set; } = EntitySkillName.WizardAttackSkill;
        protected override void InternalActivate()
        {
            this.vfxService.SpawnVFX(this.VFXName, new Vector3(5f, -1, 0), Quaternion.identity, scale: new Vector3(3, 3, 1));
            var targets = this.findTargetSystem.GetAllEnemyTarget();
            for (int i = 0; i < targets.Count; i++)
            {
                this.effectManager.Execute(targets[i], new InstantDamageTag() { Damage = this.Damage });
                this.effectManager.Execute(targets[i], new BleedTag() { Duration       = 2, Timer = 0, TimeDelay    = 0.1f });
                this.effectManager.Execute(targets[i], new SlowTag() { Duration        = 5, Timer = 0, InitialSpeed = targets[i].GetStats().GetStat<float>(StatEnum.MoveSpeed) });
            }
        }
    }
}