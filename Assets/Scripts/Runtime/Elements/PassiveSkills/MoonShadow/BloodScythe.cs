namespace Runtime.Elements.PassiveSkills.MoonShadow
{
    using System.Linq;
    using Models.Tags;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using UnityEngine;

    public class BloodScythe : IPassiveSkillPresenter
    {
        private readonly CastleManager castleManager;
        private readonly EffectManager effectManager;
        private          bool          isStartActive;
        public BloodScythe(CastleManager castleManager, EffectManager effectManager)
        {
            this.castleManager = castleManager;
            this.effectManager = effectManager;
        }
        public HeroPresenter HeroPresenter { get; set; }
        public void Init()
        {
            this.isStartActive = false;
        }
        public void Tick()
        {
            var castleModelStat            = this.castleManager.entities.First().Model.BaseStats;
            var currentCastleHealthPercent = castleModelStat.GetStat<float>(StatEnum.Health) / castleModelStat.GetStat<float>(StatEnum.MaxHealth) * 100;
            if (!(currentCastleHealthPercent < 20))
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
            this.HeroPresenter.OnAttackComplete = (target) =>
            {
                this.effectManager.AddEffectToTarget(target, new BleedTag() { Duration = 1, TimeDelay = 0.2f, Timer = 0 });
                this.HeroPresenter.OnAttackComplete = null;
            };
        }
        public void DeActiveSkill()
        {
            this.isStartActive                  = false;
            this.HeroPresenter.OnAttackComplete = null;
        }
    }
}