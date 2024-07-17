namespace Runtime.Interfaces.Skills
{
    using Runtime.Elements.Entities.Hero;

    public interface IPassiveSkillPresenter
    {
        public HeroPresenter HeroPresenter { get; set; }
        public void          Init();
        public void          Tick();
        public void          ActiveSkill();
    }
}