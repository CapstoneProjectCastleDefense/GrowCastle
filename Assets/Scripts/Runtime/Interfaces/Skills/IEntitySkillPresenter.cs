namespace Runtime.Interfaces.Skills
{
    public interface IEntitySkillPresenter
    {
        string SkillId { get; set; }
        void   Activate(IEntitySkillModel baseSkillModel);
    }

    public abstract class BaseEntitySkillPresenter<TModel> : IEntitySkillPresenter where TModel : IEntitySkillModel
    {
        public abstract string SkillId { get; set; }

        protected TModel Model;

        public virtual void Activate(IEntitySkillModel baseSkillModel)
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