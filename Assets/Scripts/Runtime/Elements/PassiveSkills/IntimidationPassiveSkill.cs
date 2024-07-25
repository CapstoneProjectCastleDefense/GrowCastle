namespace Runtime.Elements.PassiveSkills
{
    using Models.Tags;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Systems;

    public class IntimidationPassiveSkill : IPassiveSkillPresenter
    {
        private readonly FindTargetSystem findTargetSystem;
        private readonly EffectManager    effectManager;
        public           HeroPresenter    HeroPresenter { get; set; }

        public IntimidationPassiveSkill(FindTargetSystem findTargetSystem, EffectManager effectManager)
        {
            this.findTargetSystem = findTargetSystem;
            this.effectManager    = effectManager;
        }
        public void Init() { this.HeroPresenter.OnActiveSkillCasted = this.ActiveSkill; }
        public void Tick() { }
        public void ActiveSkill()
        {
            var targets = this.findTargetSystem.GetAllGroundEnemies();
            targets.ForEach(e =>
            {
                this.effectManager.AddEffectToTarget(e, new SlowTag()
                {
                    Duration = 1.5f, Timer = 0, InitialSpeed = e.GetStats().GetStat<float>(StatEnum.MaxSpeed)
                });
            });
        }
    }
}