namespace Runtime.Elements.PassiveSkills
{
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;

    public class GodLightPassiveSkill : IPassiveSkillPresenter
    {
        public  HeroPresenter HeroPresenter { get; set; }
        private float         currentBonusReduceMana;
        public  void          Init() { this.ActiveSkill(); }
        public  void          Tick() { }
        public void ActiveSkill()
        {
            this.currentBonusReduceMana = this.HeroPresenter.Model.BaseStats.GetStat<float>(StatEnum.BonusReduceMana);
            this.HeroPresenter.Model.BaseStats.SetStat(StatEnum.BonusReduceMana, this.currentBonusReduceMana + 10f);
        }
        public void DeActiveSkill()
        {
            this.HeroPresenter.Model.BaseStats.SetStat(StatEnum.BonusReduceMana, this.currentBonusReduceMana);
        }
    }
}