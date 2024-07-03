namespace Runtime.Systems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Enums;
    using Runtime.Interfaces.Abilities;
    using Runtime.Interfaces.Entities;

    public class AbilitySystem : IGameSystem
    {
        #region Inject

        private IReadOnlyDictionary<string, IAbility> abilities;

        public AbilitySystem(IReadOnlyList<IAbility> abilities) { this.abilities = abilities.ToDictionary(x => x.Id, x => x); }

        #endregion

        #region IGameSystem

        public void Initialize() { }
        public void Tick()       { }
        public void Dispose()    { }

        #endregion

        #region Ability Execute

        public void Execute(string id, ITargetable target, Dictionary<StatEnum, (Type, object)> stats)
        {
            if (!this.abilities.TryGetValue(id, out var ability)) return;

            ability.Execute(target, stats);
        }

        #endregion
    }
}