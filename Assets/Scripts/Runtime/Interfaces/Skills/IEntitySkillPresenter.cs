namespace Runtime.Interfaces.Skills
{

    public interface IEntitySkillPresenter
    {
        string         SkillId    { get; }
        void           Activate(BaseSkillModel baseSkillModel);
    }

    public abstract class BaseEntitySkillPresenter<TModel> : IEntitySkillPresenter where TModel : BaseSkillModel
    {
        public abstract string SkillId { get; }

        public TModel skillModel;

        public virtual void Activate(BaseSkillModel baseSkillModel)
        {
            if (baseSkillModel is TModel model)
            {
                this.skillModel = model;
            }
        }
    }

    public abstract class BaseSkillModel
    {

    }
}