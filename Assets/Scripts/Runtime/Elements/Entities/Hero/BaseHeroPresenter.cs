namespace Runtime.Elements.Entities.Hero
{
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.BasePoolAbleItem;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Interfaces.Skills;

    public abstract class BaseHeroPresenter : BaseGameElementPresenter<BaseHeroModel, BaseHeroView>, IHeroPresenter
    {
        public void        CastSkill(IEntitySkillPresenter entitySkillPresenter, ITargetable target) { throw new System.NotImplementedException(); }
        public void        Attack(ITargetable target)                                                { throw new System.NotImplementedException(); }
        public ITargetable FindTarget()                                                              { throw new System.NotImplementedException(); }
        public void        Equip(IEquipment equipment)                                               { throw new System.NotImplementedException(); }
        public void        Unequip(IEquipment equipment)                                             { throw new System.NotImplementedException(); }

        protected BaseHeroPresenter(BaseHeroModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }
    }
}