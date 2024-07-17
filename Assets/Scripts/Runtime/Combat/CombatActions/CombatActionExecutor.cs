namespace Runtime.Combat.CombatActions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Combat.CombatActions.Actions;
    using Runtime.Combat.CombatActions.Models;

    public class CombatActionExecutor 
    {
        private readonly Dictionary<string, ICombatAction> idToCombatAction;

        public CombatActionExecutor(List<ICombatAction> combatActions) { this.idToCombatAction = combatActions.ToDictionary(ca => ca.ActionId); }

        public void Execute(string actionId, ICombatActionModel model)
        {
            if (!this.idToCombatAction.TryGetValue(actionId, out var combatAction))
            {
                throw new Exception($"Action {actionId} not found");
            }

            combatAction.Execute(model);
        }
    }
}