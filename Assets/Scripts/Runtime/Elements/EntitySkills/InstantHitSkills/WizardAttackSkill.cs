namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;
    using Zenject;

    public class WizardAttackSkill : InstantHitSkill<BasicSkillModel>
    {
        public WizardAttackSkill(SignalBus signalBus,
                                 FindTargetSystem findTargetSystem,
                                 EffectManager effectManager,
                                 VFXService vfxService,
                                 SkillAttackBlueprint skillAttackBlueprint)
            : base(signalBus, findTargetSystem, effectManager, vfxService, skillAttackBlueprint)
        {
        }

        public override string SkillId { get; set; } = EntitySkillName.WizardAttackSkill;

        protected override void InternalCast(ICombatantPresenter caster)
        {
            this.vfxService.SpawnVFX(this.VFXName, new Vector3(5f, -1, 0), Quaternion.identity, scale: new Vector3(3, 3, 1)).Forget();
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