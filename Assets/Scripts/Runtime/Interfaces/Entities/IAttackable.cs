namespace Runtime.Interfaces.Entities
{
    using System;

    public interface IAttackable
    {
        void Attack(ITargetable target);
        ITargetable FindTarget();
        
        Type[] GetManagerTypes();
    }
}