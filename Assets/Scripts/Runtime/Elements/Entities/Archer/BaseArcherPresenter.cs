namespace Runtime.Elements.Entities.Archer
{
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.BasePoolAbleItem;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;

    public abstract class BaseArcherPresenter : BaseGameElementPresenter<BaseArcherModel, BaseArcherView>, IArcherPresenter
    {
        protected BaseArcherPresenter(BaseArcherModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager)
        {
        }
        public void Attack(ITargetable target)                                                { throw new System.NotImplementedException(); }
        public void CastSkill(IEntitySkillPresenter entitySkillPresenter, ITargetable target) { throw new System.NotImplementedException(); }
    }
}