namespace Runtime.Elements.Entities.Tower
{
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.BasePoolAbleItem;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;

    public abstract class BaseTowerPresenter : BaseGameElementPresenter<BaseTowerModel, BaseTowerView>, ITowerPresenter
    {
        protected BaseTowerPresenter(BaseTowerModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager)
        {
        }
        public void Attack(ITargetable target)                                                { throw new System.NotImplementedException(); }
        public void CastSkill(IEntitySkillPresenter entitySkillPresenter, ITargetable target) { throw new System.NotImplementedException(); }

        public void Deploy(IDeploymentTarget target) {
            throw new System.NotImplementedException();
        }

        public void Retract() {
            throw new System.NotImplementedException();
        }
    }
}