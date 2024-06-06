namespace Runtime.Interfaces.Skills
{
    using Runtime.BasePoolAbleItem;

    public interface IEntitySkillPresenter : IGameElementPresenter<IEntitySkillModel, IEntitySkillView>
    {
        void Activate();
    }
}