namespace Runtime.Interfaces.Entities
{
    using System;
    using System.Collections.Generic;
    using Runtime.Enums;

    public interface ITargetable
    {
        void                                 OnGetHit(float damage);
        void                                 OnDeath();
        ITargetable                          TargetThatImAttacking { get; set; }
        ITargetable                          TargetThatImLookingAt { get; set; }
        ITargetable                          TargetThatAttackingMe { get; set; }
        bool                                 IsDead                { get; }
        Dictionary<StatEnum, (Type, object)> GetStats();
    }
}