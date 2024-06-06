namespace Runtime.Interfaces.Skills
{
    using Runtime.BasePoolAbleItem;

    public interface IWorldSkillPresenter : IGameElementPresenter<IWorldSkillModel, IWorldSkillView>, IAbleToLevelUp
    {
    }
}