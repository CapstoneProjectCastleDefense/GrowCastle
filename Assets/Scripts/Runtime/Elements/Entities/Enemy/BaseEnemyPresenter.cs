namespace Runtime.Elements.Entities.Enemy
{
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using Runtime.Interfaces.Entities;

    public abstract class BaseEnemyPresenter : BaseElementPresenter<BaseEnemyModel, BaseEnemyView,BaseEnemyPresenter>, IEnemyPresenter
    {
        protected BaseEnemyPresenter(BaseEnemyModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager)
        {
        }
        public void        Attack(ITargetable target) { throw new System.NotImplementedException(); }
        public ITargetable FindTarget()               { throw new System.NotImplementedException(); }
        public void        OnGetHit(float damage)     { throw new System.NotImplementedException(); }
        public void        OnDeath()                  { throw new System.NotImplementedException(); }
    }
}