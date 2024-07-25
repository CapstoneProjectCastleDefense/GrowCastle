namespace Runtime.Elements.PassiveSkills
{
    using Models.Tags;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using UnityEngine;

    public class DeadlyArrowPassiveSkill : IPassiveSkillPresenter
    {
        private readonly EffectManager effectManager;
        public DeadlyArrowPassiveSkill(EffectManager effectManager) { this.effectManager = effectManager; }
        public HeroPresenter HeroPresenter { get; set; }
        public void Init()
        {
            
        }
        public void Tick()
        {
            if (this.HeroPresenter == null) return;
            if (this.HeroPresenter.AttackCount == 4)
            {
                this.ActiveSkill();
                this.HeroPresenter.AttackCount = 0;
            }
        }
        public void ActiveSkill()
        {
            var result = Random.Range(0, 100);
            if (result <= 80)
            {
                this.HeroPresenter.OnAttackComplete = (target) =>
                {
                    this.effectManager.AddEffectToTarget(target, new DeathEffectTag() { HpPercentRemainToTrigger = 100 });
                };
            }
        }
    }
}