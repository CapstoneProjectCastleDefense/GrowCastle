namespace Runtime.Combat.CombatActions
{
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Combat.CombatActions.Actions;
    using Runtime.Combat.CombatActions.Models;

    public class CombatActionExecutor
    {
        private readonly IReadOnlyDictionary<string, ICombatAction> idToCombatAction;

        public CombatActionExecutor(IEnumerable<ICombatAction> combatActions)
        {
            this.idToCombatAction = combatActions.ToDictionary(ca => ca.ActionId, ca => ca);
        }
        
        public void Execute<TModel>(string actionId, TModel model) where TModel : ICombatActionModel
        {
            if (this.idToCombatAction.TryGetValue(actionId, out var combatAction))
            {
                combatAction.Execute(model);
            }
        }
    }
}