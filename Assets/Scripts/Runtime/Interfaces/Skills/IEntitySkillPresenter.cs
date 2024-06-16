namespace Runtime.Interfaces.Skills
{
    using Runtime.Enums;

    public interface IEntitySkillPresenter
    {
        EntitySkillType SkillType { get; set; }
        void            Activate(BaseSkillModel baseSkillModel);
    }

    public abstract class BaseEntitySkillPresenter<TModel> : IEntitySkillPresenter where TModel : BaseSkillModel
    {
        public abstract EntitySkillType SkillType { get; set; }
        
        protected       TModel          SkillModel;

        public void Activate(BaseSkillModel baseSkillModel)
        {
            if (baseSkillModel is TModel model)
            {
                this.SkillModel = model;
            }

            this.InternalActivate();
        }

        protected virtual void InternalActivate() { }
    }

    public abstract class BaseSkillModel
    {
    }
}