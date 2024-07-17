namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.Systems;
    using Zenject;

    public abstract class BaseHeroInstantHitSkill : InstantHitSkill
    {
        protected BaseHeroInstantHitSkill(SignalBus signalBus, FindTargetSystem findTargetSystem, EffectManager effectManager, VFXService vfxService, SkillAttackBlueprint skillAttackBlueprint) : base(
            signalBus, findTargetSystem, effectManager, vfxService, skillAttackBlueprint)
        {
        }

        public override void Activate(ICombatantPresenter combatant)
        {
            base.Activate(combatant);
            var hero = (HeroPresenter)combatant;
            hero.OnClickAction = () => this.InternalCast(combatant);
        }

        public override void Deactivate(ICombatantPresenter combatant)
        {
            base.Deactivate(combatant);
            var hero = (HeroPresenter)combatant;
            hero.OnClickAction = null;
        }
    }
}