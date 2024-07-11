namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using System;
    using System.Collections.Generic;
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

    public class GreenLeafInstantHitSkill : InstantHitSkill<BasicSkillModel>
    {
        private readonly VFXService       vfxService;
        private readonly EnemyManager     enemyManager;
        private readonly FindTargetSystem findTargetSystem;
        private readonly AbilitySystem    abilitySystem;
        private readonly EffectManager    effectManager;
        public GreenLeafInstantHitSkill(
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
        public override string SkillId { get; set; } = EntitySkillName.GreenLeafAttack;
        protected override  void InternalActivate()
        {
            this.vfxService.SpawnVFX(this.VFXName, new Vector3(-3, -1, 0), Quaternion.identity, scale: new Vector3(2, 2, 1));
            var targets = this.findTargetSystem.GetAllEnemyTarget();
            for (int i = 0; i < targets.Count; i++)
            {
                this.effectManager.Execute(targets[i], new InstantDamageTag() { Damage = this.Damage });
                this.effectManager.Execute(targets[i], new SlowTag() { Duration        = 3, Timer = 0, InitialSpeed = targets[i].GetStats().GetStat<float>(StatEnum.MoveSpeed) });
            }
        }
    }
}