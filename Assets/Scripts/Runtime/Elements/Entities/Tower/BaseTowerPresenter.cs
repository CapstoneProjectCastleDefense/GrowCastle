namespace Runtime.Elements.Entities.Tower
{
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;

    public abstract class BaseTowerPresenter<TModel, TView, TPresenter> : BaseElementPresenter<TModel, TView, TPresenter>, ITowerPresenter
        where TView : BaseTowerView
        where TPresenter : BaseTowerPresenter<TModel, TView, TPresenter>
        where TModel : BaseTowerModel
    {
        protected BaseTowerPresenter(TModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }
        public void        Attack(ITargetable target)                                                { throw new System.NotImplementedException(); }
        public ITargetable FindTarget()                                                              { throw new System.NotImplementedException(); }
        public void        CastSkill(string skillId, ITargetable target) { throw new System.NotImplementedException(); }
    }
}