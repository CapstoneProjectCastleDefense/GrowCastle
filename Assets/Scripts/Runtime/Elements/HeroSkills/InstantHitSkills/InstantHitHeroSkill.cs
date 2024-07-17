namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using Models.Blueprints;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.Systems;
    using Zenject;

    public abstract class InstantHitHeroSkill<TModel> : BaseHeroSkill where TModel : BasicHeroSkillModel
    {
        private readonly SkillAttackBlueprint skillAttackBlueprint;
        protected        string               VFXName;
        protected        float                Damage;

        protected InstantHitHeroSkill(SignalBus signalBus,
                                      FindTargetSystem findTargetSystem,
                                      EffectManager effectManager,
                                      VFXService vfxService,
                                      SkillAttackBlueprint skillAttackBlueprint)
            : base(signalBus, findTargetSystem, effectManager, vfxService)
        {
            this.skillAttackBlueprint = skillAttackBlueprint;
        }

        public override void Activate(HeroPresenter caster)
        {
            // this.VFXName = this.skillAttackBlueprint.GetDataById(this.Model.Id).LevelToConfigRecords[this.Model.Level].PrefabName;
            // this.Damage  = this.skillAttackBlueprint.GetDataById(this.Model.Id).LevelToConfigRecords[this.Model.Level].Damage;
            this.InternalCast(caster);
        }

        public override void Deactivate(HeroPresenter hero)
        {
            
        }

        protected abstract void InternalCast(HeroPresenter caster);
    }
}