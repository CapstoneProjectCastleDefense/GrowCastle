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
    using Zenject;

    public class GrayMageInstantHitSkill : InstantHitSkill<BasicSkillModel>
    {
        public GrayMageInstantHitSkill(SignalBus signalBus,
                                       FindTargetSystem findTargetSystem,
                                       EffectManager effectManager,
                                       VFXService vfxService,
                                       SkillAttackBlueprint skillAttackBlueprint)
            : base(signalBus, findTargetSystem, effectManager, vfxService, skillAttackBlueprint)
        {
        }

        public override string SkillId { get; set; } = EntitySkillName.GrayMageAttack;

        protected override void InternalActivate()
        {
            this.vfxService.SpawnVFX(this.VFXName, new Vector3(-1f, -1, 0), Quaternion.identity, scale: new Vector3(5, 5, 1));
            var targets = this.findTargetSystem.GetAllEnemyTarget();
            for (int i = 0; i < targets.Count; i++)
            {
                this.effectManager.Execute(targets[i], new InstantDamageTag() { Damage = this.Damage });
                this.effectManager.Execute(targets[i], new BleedTag() { Duration       = 3, Timer = 0, TimeDelay    = 0.2f });
                this.effectManager.Execute(targets[i], new SlowTag() { Duration        = 1, Timer = 0, InitialSpeed = targets[i].GetStats().GetStat<float>(StatEnum.MoveSpeed) });
            }
        }
    }
}