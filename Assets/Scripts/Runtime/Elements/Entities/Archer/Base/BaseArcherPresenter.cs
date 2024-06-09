namespace Runtime.Elements.Entities.Archer.Base
{
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.BasePoolAbleItem;
    using Runtime.Enums;
    using Runtime.Interfaces;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;

    public abstract class BaseArcherPresenter : BaseGameElementPresenter<BaseArcherModel, BaseArcherView>, IArcherPresenter
    {
        protected BaseArcherPresenter(BaseArcherModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }
        public void Attack(ITargetable target = null)
        {
            if (target == null)
                target = this.FindTarget();
            if (target == null) return;
            var attackPower = this.Model.GetStat<float>(StatEnum.Attack);
            
        }
        public ITargetable FindTarget()
        {
            var attackPriority = this.Model.GetStat<AttackPriorityEnum>(StatEnum.AttackPriority);
            return null;
        }
        public void CastSkill(IEntitySkillPresenter entitySkillPresenter, ITargetable target) { throw new System.NotImplementedException(); }
    }
}