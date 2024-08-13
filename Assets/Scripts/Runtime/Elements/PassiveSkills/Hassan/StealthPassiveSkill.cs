namespace Runtime.Elements.PassiveSkills.Hassan
{
    using Models.Tags;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;

    public class StealthPassiveSkill : IPassiveSkillPresenter
    {
        private readonly EffectManager effectManager;

        public StealthPassiveSkill(EffectManager effectManager)
        {
            this.effectManager = effectManager;
        }

        public  HeroPresenter HeroPresenter { get; set; }
        private float         currentAtk;

        public void Init()
        {
            this.ActiveSkill();
        }

        public void Tick()
        {
        }

        public void ActiveSkill()
        {
            this.currentAtk = this.HeroPresenter.Model.GetStat<float>(StatEnum.Attack);
            this.HeroPresenter.Model.SetStat(StatEnum.Attack, this.currentAtk * 1.2f);
            this.HeroPresenter.OnAttackComplete += this.TriggerEffect;
        }

        public void DeActiveSkill()
        {
            this.HeroPresenter.Model.SetStat(StatEnum.Attack, this.currentAtk);
            this.HeroPresenter.OnAttackComplete -= this.TriggerEffect;
        }

        private void TriggerEffect(ITargetable targetable)
        {
            this.effectManager.AddEffectToTarget(targetable, new BleedTag() { Duration = 1, Timer = 0, TimeDelay = 0.15f });
        }
    }
}