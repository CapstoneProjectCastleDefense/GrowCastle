namespace Runtime.Elements.Entities.Hero
{
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Interfaces.Skills;

    public abstract class BaseHeroPresenter<TModel, TView, TPresenter> : BaseElementPresenter<TModel, TView,TPresenter>, IHeroPresenter
    where TView : BaseHeroView
    where TPresenter : BaseHeroPresenter<TModel, TView, TPresenter>
    where TModel : BaseHeroModel
    {
        protected BaseHeroPresenter(TModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }
        public void        CastSkill(IEntitySkillPresenter entitySkillPresenter, ITargetable target) { throw new System.NotImplementedException(); }
        public void        Attack(ITargetable target)                                                { throw new System.NotImplementedException(); }
        public ITargetable FindTarget()                                                              { throw new System.NotImplementedException(); }
        public void        Equip(IEquipment equipment)                                               { throw new System.NotImplementedException(); }
        public void        Unequip(IEquipment equipment)                                             { throw new System.NotImplementedException(); }
    }
}