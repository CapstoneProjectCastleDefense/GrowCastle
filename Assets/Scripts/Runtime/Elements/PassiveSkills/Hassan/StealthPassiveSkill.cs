namespace Runtime.Elements.PassiveSkills.Hassan
{
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;

    public class StealthPassiveSkill : IPassiveSkillPresenter
    {
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
            var currentAtk = this.HeroPresenter.Model.GetStat<float>(StatEnum.Attack);
            this.HeroPresenter.Model.SetStat(StatEnum.Attack,currentAtk*1.2f);
        }
    }
}