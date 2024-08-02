namespace Runtime.Elements.PassiveSkills.Storm
{
    using System.Linq;
    using Models.Tags;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Systems;

    public class ForesightPassiveSkill : IPassiveSkillPresenter
    {
        private readonly EffectManager    effectManager;
        private readonly FindTargetSystem findTargetSystem;

        public ForesightPassiveSkill(EffectManager effectManager, FindTargetSystem findTargetSystem)
        {
            this.effectManager    = effectManager;
            this.findTargetSystem = findTargetSystem;
        }

        public HeroPresenter HeroPresenter { get; set; }

        public void Init()
        {
            this.ActiveSkill();
        }

        public void Tick()
        {
        }

        public void ActiveSkill()
        {
            this.HeroPresenter.OnAttackComplete += (target) =>
            {
                if (target.Tags.Contains(ElementTag.Range))
                {
                    this.effectManager.AddEffectToTarget(target, new InstantDamageTag() { Damage = this.HeroPresenter.Model.GetStat<float>(StatEnum.Attack) * 0.3f });
                }
            };
        }
    }
}