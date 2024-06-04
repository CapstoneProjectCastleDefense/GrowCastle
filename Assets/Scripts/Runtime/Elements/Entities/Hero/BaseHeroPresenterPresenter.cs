namespace Runtime.Elements.Entities.Hero
{
    using System.Collections.Generic;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.BasePoolAbleItem;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Interfaces.Skills;

    public abstract class BaseHeroPresenterPresenter : BasePoolableItemPresenter<BaseHeroModel, BaseHeroView>, IHeroPresenter
    {
        public void CastSkill(ISkillPresenter skillPresenter, ITargetable target) { throw new System.NotImplementedException(); }
        public void Attack(ITargetable target)                                    { throw new System.NotImplementedException(); }
        public void Equip(IEquipment equipment)                                   { throw new System.NotImplementedException(); }
        public void Unequip(IEquipment equipment)                                 { throw new System.NotImplementedException(); }

        protected BaseHeroPresenterPresenter(BaseHeroModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }
    }
}