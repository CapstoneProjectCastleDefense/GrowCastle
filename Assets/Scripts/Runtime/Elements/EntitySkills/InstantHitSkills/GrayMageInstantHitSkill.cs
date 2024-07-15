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

        protected override void InternalCast(ICombatantPresenter caster)
        {
            this.vfxService.SpawnVFX(this.VFXName, new Vector3(-1f, -1, 0), Quaternion.identity, scale: new Vector3(5, 5, 1)).Forget();
            var targets = this.findTargetSystem.GetAllEnemyTarget();
            foreach (var t in targets)
            {
                this.effectManager.Execute(t, new InstantDamageTag() { Damage = this.Damage });
                this.effectManager.Execute(t, new BleedTag() { Duration       = 3, Timer = 0, TimeDelay    = 0.2f });
                this.effectManager.Execute(t, new SlowTag() { Duration        = 1, Timer = 0, InitialSpeed = t.GetStats().GetStat<float>(StatEnum.MoveSpeed) });
            }
        }
    }
}