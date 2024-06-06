namespace Runtime.Elements.Entities.Leader
{
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.BasePoolAbleItem;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Interfaces.Skills;

    public abstract class BaseLeaderPresenter : BaseGameElementPresenter<BaseLeaderModel, BaseLeaderView>, ILeaderPresenter
    {
        public void Attack(ITargetable target)                                                { throw new System.NotImplementedException(); }
        public void OnGetHit(float damage)                                                    { throw new System.NotImplementedException(); }
        public void OnDeath()                                                                 { throw new System.NotImplementedException(); }
        public void CastSkill(IEntitySkillPresenter entitySkillPresenter, ITargetable target) { throw new System.NotImplementedException(); }
        public void Equip(IEquipment equipment)                                               { throw new System.NotImplementedException(); }
        public void Unequip(IEquipment equipment)                                             { throw new System.NotImplementedException(); }
        protected BaseLeaderPresenter(BaseLeaderModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }
    }
}