namespace Runtime.Systems.Effects
{
    using System;
    using System.Collections.Generic;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;

    public interface IAffectSystem : IGameSystem
    {
        Type              ConditionFilterTag { get; }
        List<ITargetable> AffectedElements   { get; set; }
        void              Filter();
        void              TriggerEffect(ITargetable target);
        AffectManager     AffectManager { set; }
    }
}