namespace Runtime.Interfaces.Skills
{
    using Runtime.BasePoolAbleItem;

    public interface ISkillPresenter : IPoolableItemPresenter
    {
        void Activate();
    }
}