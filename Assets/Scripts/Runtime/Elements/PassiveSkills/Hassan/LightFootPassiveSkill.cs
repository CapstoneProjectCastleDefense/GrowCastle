namespace Runtime.Elements.PassiveSkills.Hassan
{
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;

    public class LightFootPassiveSkill : IPassiveSkillPresenter
    {
        public HeroPresenter HeroPresenter { get; set; }
        public void          Init()        { this.ActiveSkill(); }
        public void          Tick()        { }
        public void ActiveSkill()
        {
            var currentAttackSpeed = this.HeroPresenter.Model.GetStat<float>(StatEnum.AttackSpeed);
            this.HeroPresenter.Model.SetStat(StatEnum.AttackSpeed, currentAttackSpeed * 1.4f);
        }
    }
}