namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.Systems;
    using Zenject;

    public abstract class InstantHitSkill : BaseSkill
    {
        private readonly SkillAttackBlueprint skillAttackBlueprint;
        protected        string               VFXName;
        protected        float                Damage;

        protected InstantHitSkill(SignalBus signalBus,
                                      FindTargetSystem findTargetSystem,
                                      EffectManager effectManager,
                                      VFXService vfxService,
                                      SkillAttackBlueprint skillAttackBlueprint)
            : base(signalBus, findTargetSystem, effectManager, vfxService)
        {
            this.skillAttackBlueprint = skillAttackBlueprint;
        }

        public override void Activate(ICombatantPresenter combatant)
        {
            this.VFXName = this.skillAttackBlueprint.GetDataById(this.SkillId).LevelToConfigRecords[1].PrefabName;
            this.Damage  = this.skillAttackBlueprint.GetDataById(this.SkillId).LevelToConfigRecords[1].Damage;
        }

        public override void Deactivate(ICombatantPresenter combatant)
        {
            
        }

        protected abstract void InternalCast(ICombatantPresenter combatant);
    }
}