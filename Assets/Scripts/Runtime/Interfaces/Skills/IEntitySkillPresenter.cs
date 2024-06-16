namespace Runtime.Interfaces.Skills
{
    using Runtime.Enums;

    public interface IEntitySkillPresenter
    {
        EntitySkillType SkillType { get; set; }
        void            Activate(IEntitySkillModel baseSkillModel);
    }

    public abstract class BaseEntitySkillPresenter<TModel> : IEntitySkillPresenter where TModel : IEntitySkillModel
    {
        public abstract EntitySkillType SkillType { get; set; }

        protected TModel Model;

        public void Activate(IEntitySkillModel baseSkillModel)
        {
            if (baseSkillModel is TModel model)
            {
                this.Model = model;
            }

            this.InternalActivate();
        }

        protected abstract void InternalActivate();
    }
}