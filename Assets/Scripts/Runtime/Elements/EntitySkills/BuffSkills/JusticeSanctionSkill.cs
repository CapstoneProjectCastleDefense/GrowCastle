namespace Runtime.Elements.EntitySkills.BuffSkills
{
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;

    public class JusticeSanctionSkill : BaseEntitySkillPresenter<BasicSkillModel>
    {
        private readonly FindTargetSystem findTargetSystem;
        private readonly EffectManager    effectManager;
        private readonly VFXService       vfxService;

        public JusticeSanctionSkill(FindTargetSystem findTargetSystem, EffectManager effectManager, VFXService vfxService)
        {
            this.findTargetSystem = findTargetSystem;
            this.effectManager    = effectManager;
            this.vfxService       = vfxService;
        }
        public override string SkillId { get; set; } = EntitySkillName.JusticeSanction;
        protected override void InternalActivate()
        {
            this.vfxService?.SpawnVFX("FrostDeath", new Vector3(3.5f, -1, 0),scale:new Vector3(4,4,4));
            var targets = this.findTargetSystem.GetAllGroundEnemies();
            targets.ForEach(e=>this.effectManager.AddEffectToTarget(e,new FreezeTag(){Duration = 2,InitialSpeed = e.GetStats().GetStat<float>(StatEnum.MoveSpeed),Timer = 0}));
        }
    }
}