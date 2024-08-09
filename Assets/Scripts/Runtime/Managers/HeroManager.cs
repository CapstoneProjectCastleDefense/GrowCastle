namespace Runtime.Managers
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Managers.Base;
    using Runtime.Services;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using UnityEngine;

    public class HeroManager : BaseElementManager<HeroModel, HeroPresenter, HeroView>
    {
        private readonly SkillBlueprint            skillBlueprint;
        private readonly HeroBlueprint             heroBlueprint;
        private readonly HeroUpgradeService        heroUpgradeService;
        private readonly TalentLocalDataController talentLocalDataController;
        private          GameStateMachine          gameStateMachine;

        public HeroManager(BaseElementPresenter<HeroModel, HeroView, HeroPresenter>.Factory factory, SkillBlueprint skillBlueprint, HeroBlueprint heroBlueprint, HeroUpgradeService heroUpgradeService, TalentLocalDataController talentLocalDataController)
            : base(factory)
        {
            this.skillBlueprint            = skillBlueprint;
            this.heroBlueprint             = heroBlueprint;
            this.heroUpgradeService        = heroUpgradeService;
            this.talentLocalDataController = talentLocalDataController;
        }

        public HeroPresenter CreateSingleHero(string id, Transform parent)
        {
            var heroPresenter = this.CreateElement(new()
            {
                Id         = id,
                ParentView = parent,
                BaseStats  = this.GetCurrentStatOfHero(id),
            });
            heroPresenter.UpdateView().Forget();
            heroPresenter.SetManager(this);

            return heroPresenter;
        }

        private Dictionary<StatEnum, (Type, object)> GetCurrentStatOfHero(string id)
        {
            return new()
            {
                { StatEnum.Attack, (typeof(float), this.heroUpgradeService.GetCurrentAttack(id) + this.talentLocalDataController.GetTalentEffect(TalentType.IncreaseHeroAttack) * this.heroUpgradeService.GetCurrentAttack(id)) },
                { StatEnum.Health, (typeof(float), 10f) },
                { StatEnum.AttackSpeed, (typeof(float), 1f) },
                { StatEnum.BonusReduceMana, (typeof(float), 0f) },
                { StatEnum.AttackPriority, (typeof(AttackPriorityEnum), AttackPriorityEnum.Ground) },
                { StatEnum.ActiveSkillCooldown, (typeof(float), this.skillBlueprint[this.heroBlueprint[id].ActiveSkill.skillName].Cooldown) },
            };
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
                if (canAttack)
                {
                    e.Model.BaseStats = this.GetCurrentStatOfHero(e.Model.Id);
                }
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