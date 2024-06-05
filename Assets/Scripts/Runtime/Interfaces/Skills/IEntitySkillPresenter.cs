namespace Runtime.Interfaces.Skills
{
    using Runtime.BasePoolAbleItem;

    public interface IEntitySkillPresenter : IPoolableItemPresenter
    {
        void Activate();
    }
}