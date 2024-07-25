namespace Runtime.Elements.PassiveSkills
{
    using Runtime.Elements.Entities.Hero;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;

    public class TheFearPassiveSkill : IPassiveSkillPresenter
    {
        private readonly CastleManager castleManager;
        private readonly EffectManager effectManager;
        public TheFearPassiveSkill(CastleManager castleManager, EffectManager effectManager)
        {
            this.castleManager = castleManager;
            this.effectManager = effectManager;
        }
        public HeroPresenter HeroPresenter { get; set; }
        public void Init()
        {
            
        }
        public void Tick()
        {
            
        }
        public void ActiveSkill()
        {
            
        }
    }
}