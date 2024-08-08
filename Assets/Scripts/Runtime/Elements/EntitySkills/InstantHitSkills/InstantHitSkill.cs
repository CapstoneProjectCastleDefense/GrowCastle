namespace Runtime.Elements.EntitySkills.InstantHitSkills
{
    using Models.Blueprints;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;

    public abstract class InstantHitSkill<TModel> : BaseEntitySkillPresenter<TModel> where TModel : BasicSkillModel
    {
        private readonly SkillAttackBlueprint skillAttackBlueprint;
        protected        string               VFXName;
        protected        float                Damage;

        protected InstantHitSkill(SkillAttackBlueprint skillAttackBlueprint) { this.skillAttackBlueprint = skillAttackBlueprint; }

        public override void Activate(IEntitySkillModel baseSkillModel)
        {
            if (baseSkillModel is TModel model)
            {
                this.Model = model;
            }
            this.VFXName = this.skillAttackBlueprint.GetDataById(this.Model.Id).LevelToConfigRecords[this.Model.Level].PrefabName;
            this.Damage  = ((ITargetable)this.Model.Caster).GetStats().GetStat<float>(StatEnum.Attack) * this.skillAttackBlueprint.GetDataById(this.Model.Id).LevelToConfigRecords[this.Model.Level].Damage / 100;
            this.InternalActivate();
        }
    }
}