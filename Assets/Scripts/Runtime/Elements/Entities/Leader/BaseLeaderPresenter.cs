namespace Runtime.Elements.Entities.Leader
{
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Interfaces.Skills;

    public abstract class BaseLeaderPresenter<TModel, TView, TPresenter> : BaseElementPresenter<TModel, TView, TPresenter>, ILeaderPresenter
        where TView : BaseLeaderView
        where TPresenter : BaseLeaderPresenter<TModel, TView, TPresenter>
        where TModel : BaseLeaderModel
    {
        protected BaseLeaderPresenter(TModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }

        public void Attack(ITargetable target) { throw new System.NotImplementedException(); }

        public ITargetable FindTarget() { throw new System.NotImplementedException(); }

        public void CastSkill(string skillId, ITargetable target) { throw new System.NotImplementedException(); }

        public void Equip(IEquipment equipment) { throw new System.NotImplementedException(); }

        public void UnEquip(IEquipment equipment) { throw new System.NotImplementedException(); }

        public void OnGetHit(float damage) { throw new System.NotImplementedException(); }

        public void OnDeath() { throw new System.NotImplementedException(); }
    }
}