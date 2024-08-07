namespace Runtime.Managers
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Managers.Base;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using UnityEngine;

    public class HeroManager : BaseElementManager<HeroModel, HeroPresenter, HeroView>
    {
        private readonly SkillBlueprint   skillBlueprint;
        private readonly HeroBlueprint    heroBlueprint;
        private          GameStateMachine gameStateMachine;
        public HeroManager(BaseElementPresenter<HeroModel, HeroView, HeroPresenter>.Factory factory, SkillBlueprint skillBlueprint, HeroBlueprint heroBlueprint)
            : base(factory)
        {
            this.skillBlueprint = skillBlueprint;
            this.heroBlueprint  = heroBlueprint;
        }

        public HeroPresenter CreateSingleHero(string id, Transform parent)
        {
            var heroPresenter = this.CreateElement(new()
            {
                Id         = id,
                ParentView = parent,
                Stats = new()
                {
                    { StatEnum.Attack, (typeof(float), 2f) },
                    { StatEnum.Health, (typeof(float), 10f) },
                    { StatEnum.AttackSpeed, (typeof(float), 1f) },
                    { StatEnum.BonusReduceMana, (typeof(float), 1f) },
                    { StatEnum.AttackPriority, (typeof(AttackPriorityEnum), AttackPriorityEnum.Ground) },
                    { StatEnum.ActiveSkillCooldown, (typeof(float),this.skillBlueprint[this.heroBlueprint[id].ActiveSkill.skillName].Cooldown)}
                },
            });
            heroPresenter.UpdateView().Forget();
            heroPresenter.SetManager(this);
            return heroPresenter;
        }

        public override void Tick()
        {
            base.Tick();
            if (this.gameStateMachine.CurrentState is GamePrepareState)
            {
                this.entities.ForEach(e =>
                {
                    e.SetRaycastActive(false);
                });
                return;
            }
            this.entities.ForEach(e =>
            {
                e.SetRaycastActive(true);
            });

        }

        public void ChangeAttackStatusOfAllHero(bool canAttack)
        {
            this.entities.ForEach(e =>
            {
                if (!canAttack) e.ResetCooldown();
                e.SetAttackStatus(canAttack);
                e.SetRaycastActive(canAttack);
            });
        }

        public override void Initialize()
        {
            this.gameStateMachine = this.GetCurrentContainer().Resolve<GameStateMachine>();
        }
    }
}