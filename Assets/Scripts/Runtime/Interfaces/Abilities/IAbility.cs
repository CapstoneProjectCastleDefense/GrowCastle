namespace Runtime.Interfaces.Abilities
{
    using System;
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;

    public interface IAbility : IIdentifier
    {
        public void Execute(ITargetable target, Dictionary<StatEnum, (Type, object)> stats);
    }
}