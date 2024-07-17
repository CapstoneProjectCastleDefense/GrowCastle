namespace Runtime.Combat.CombatActions.Actions
{
    using Runtime.Combat.CombatActions.Models;

    public class InstantHit : ICombatAction<InstantHitModel>
    {
        public string ActionId                       { get; set; } = CombatActionId.InstantHit;

        public void Execute(InstantHitModel model)
        {
            
        }
    }
}