namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using Models.Blueprints;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.Systems;
    using Zenject;

    public abstract class InstantHitSkill<TModel> : BaseEntitySkillPresenter<TModel> where TModel : BasicSkillModel
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
        
        public override void Activate(IEntitySkillModel baseSkillModel)
        {
            if (baseSkillModel is TModel model)
            {
                this.Model = model;
            }
            this.VFXName       = this.skillAttackBlueprint.GetDataById(this.Model.Id).LevelToConfigRecords[this.Model.Level].PrefabName;
            this.Damage        = this.skillAttackBlueprint.GetDataById(this.Model.Id).LevelToConfigRecords[this.Model.Level].Damage;
            this.InternalActivate();
        }
    }

}