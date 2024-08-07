namespace Runtime.Elements.PassiveSkills.Shadow
{
    using System.Linq;
    using Models.Tags;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using UnityEngine;

    public class TheFearPassiveSkill : IPassiveSkillPresenter
    {
        private readonly CastleManager castleManager;
        private readonly EffectManager effectManager;
        public TheFearPassiveSkill(CastleManager castleManager, EffectManager effectManager)
        {
            this.castleManager = castleManager;
            this.effectManager = effectManager;
        }
        public  HeroPresenter HeroPresenter { get; set; }
        private bool          isStartActive;
        public void Init()
        {
            
        }
        public void Tick()
        {
            var castleModelStat            = this.castleManager.entities.First().Model.BaseStats;
            var currentCastleHealthPercent = castleModelStat.GetStat<float>(StatEnum.Health) / castleModelStat.GetStat<float>(StatEnum.MaxHealth) * 100;
            if (!(currentCastleHealthPercent < 30))
            {
                this.isStartActive                  = false;
                this.HeroPresenter.OnAttackComplete = null;
                return;
            }

            if (this.isStartActive) return;
            this.ActiveSkill();
            this.isStartActive = true;

        }
        public void ActiveSkill()
        {
            Debug.Log($"active skill {this.GetType().FullName}");
            var result = Random.Range(0, 100);
            if (result <= 30)
            {
                this.HeroPresenter.OnAttackComplete += this.AddFearEffect;
            }
        }
        public void DeActiveSkill()
        {
            this.HeroPresenter.OnAttackComplete -= this.AddFearEffect;
        }
        private void AddFearEffect(ITargetable target)
        {
            this.effectManager.AddEffectToTarget(target, new FearTag() { Duration = 2, Timer = 0});
        }
    }
}