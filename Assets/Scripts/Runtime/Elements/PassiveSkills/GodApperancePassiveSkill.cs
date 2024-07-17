namespace Runtime.Elements.PassiveSkills
{
    using Runtime.Elements.Entities.Hero;
    using Runtime.Interfaces.Skills;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;

    public class GodApperancePassiveSkill : IPassiveSkillPresenter
    {
        private readonly EntitySkillSystem entitySkillSystem;
        public GodApperancePassiveSkill(EntitySkillSystem entitySkillSystem) { this.entitySkillSystem = entitySkillSystem; }

        public HeroPresenter HeroPresenter { get; set; }
        public void Init()
        {
        }
        public void Tick()
        {
            if (this.HeroPresenter == null) return;
            if (this.HeroPresenter.AttackCount == 3)
            {
                this.ActiveSkill();
                this.HeroPresenter.AttackCount = 0;
            }
        }
        public void ActiveSkill()
        {
            this.entitySkillSystem.CastSkill(EntitySkillName.GreenLeafAttack, new BasicSkillModel()
            {
                Level = 1,
                Id    = EntitySkillName.GreenLeafAttack
            });
        }
    }
}