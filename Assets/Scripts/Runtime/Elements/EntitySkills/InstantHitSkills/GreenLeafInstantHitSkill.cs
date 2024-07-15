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

    public class GreenLeafInstantHitSkill : InstantHitSkill<BasicSkillModel>
    {
        public GreenLeafInstantHitSkill(SignalBus signalBus,
                                        FindTargetSystem findTargetSystem,
                                        EffectManager effectManager,
                                        VFXService vfxService,
                                        SkillAttackBlueprint skillAttackBlueprint)
            : base(signalBus, findTargetSystem, effectManager, vfxService, skillAttackBlueprint)
        {
        }

        public override string SkillId { get; set; } = EntitySkillName.GreenLeafAttack;

        protected override void InternalCast(ICombatantPresenter caster)
        {
            this.vfxService.SpawnVFX(this.VFXName, new Vector3(-3, -1, 0), Quaternion.identity, scale: new Vector3(2, 2, 1)).Forget();
            var targets = this.findTargetSystem.GetAllEnemyTarget();
            for (int i = 0; i < targets.Count; i++)
            {
                this.effectManager.Execute(targets[i], new InstantDamageTag() { Damage = this.Damage });
                this.effectManager.Execute(targets[i], new SlowTag() { Duration        = 3, Timer = 0, InitialSpeed = targets[i].GetStats().GetStat<float>(StatEnum.MoveSpeed) });
            }
        }
    }
}