namespace Runtime.Elements.PassiveSkills.MoonShadow
{
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;

    public class PiercingScythe : IPassiveSkillPresenter
    {
        public  HeroPresenter HeroPresenter { get; set; }
        private float         baseHeroAtk;
        public  void          Init() { this.ActiveSkill(); }
        public void Tick()
        {
            if (this.HeroPresenter.AttackCount <= 5) return;
            this.HeroPresenter.Model.SetStat(StatEnum.Attack, this.baseHeroAtk * 1.2f);
            this.HeroPresenter.AttackCount = 0;
        }
        public void ActiveSkill()
        {
            this.baseHeroAtk                    = this.HeroPresenter.Model.GetStat<float>(StatEnum.Attack);
            this.HeroPresenter.OnAttackComplete = (target) => { this.HeroPresenter.Model.SetStat(StatEnum.Attack, this.baseHeroAtk); };
        }
    }
}