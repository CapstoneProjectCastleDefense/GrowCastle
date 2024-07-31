namespace Runtime.Elements.PassiveSkills
{
    using System;
    using Cysharp.Threading.Tasks;
    using Models.Tags;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Systems;

    public class ControlGravityPassiveSkill : IPassiveSkillPresenter
    {
        private readonly FindTargetSystem findTargetSystem;
        private readonly EffectManager    effectManager;
        public           HeroPresenter    HeroPresenter { get; set; }

        public ControlGravityPassiveSkill(FindTargetSystem findTargetSystem, EffectManager effectManager)
        {
            this.findTargetSystem = findTargetSystem;
            this.effectManager    = effectManager;
        }
        public void Init() { this.HeroPresenter.OnActiveSkillCasted = this.ActiveSkill; }
        public void Tick() { }
        public async void ActiveSkill()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(2));
            var targets = this.findTargetSystem.GetAllFlyEnemies();
            targets.ForEach(e =>
            {
                this.effectManager.AddEffectToTarget(e, new SlowTag()
                {
                    Duration = 1.5f, Timer = 0
                });
            });
        }
    }
}