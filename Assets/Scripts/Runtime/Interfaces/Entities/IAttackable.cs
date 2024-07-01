namespace Runtime.Interfaces.Entities
{
    using System;

    public interface IAttackable
    {
        void Attack(ITargetable target);
        ITargetable FindTarget();
        
        float AttackCooldownTime { get; }
        
        Type[] GetManagerTypes();
        string[] GetTags();
    }
}