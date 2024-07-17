namespace Runtime.Elements.PassiveSkills
{
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;

    public class GodLightPassiveSkill : IPassiveSkillPresenter
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
            this.HeroPresenter.Model.Stats.SetStat(StatEnum.BonusReduceMana,10);
        }
    }
}