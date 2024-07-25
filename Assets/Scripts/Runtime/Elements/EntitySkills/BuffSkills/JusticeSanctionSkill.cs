namespace Runtime.Elements.EntitySkills.BuffSkills
{
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.StaticValues;
    using Runtime.Systems;

    public class JusticeSanctionSkill : BaseEntitySkillPresenter<BasicSkillModel>
    {
        private readonly FindTargetSystem findTargetSystem;
        private readonly EffectManager    effectManager;
        public JusticeSanctionSkill(FindTargetSystem findTargetSystem, EffectManager effectManager)
        {
            this.findTargetSystem = findTargetSystem;
            this.effectManager    = effectManager;
        }
        public override string SkillId { get; set; } = EntitySkillName.JusticeSanction;
        protected override void InternalActivate()
        {
            var targets = this.findTargetSystem.GetAllGroundEnemies();
            targets.ForEach(e=>this.effectManager.AddEffectToTarget(e,new FreezeTag(){Duration = 2,InitialSpeed = e.GetStats().GetStat<float>(StatEnum.MoveSpeed),Timer = 0}));
        }
    }
}