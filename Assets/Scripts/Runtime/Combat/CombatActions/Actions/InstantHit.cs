namespace Runtime.Combat.CombatActions.Actions
{
    using Runtime.Combat.CombatActions.Models;

    public class InstantHit : ICombatAction
    {
        public string ActionId { get; set; } = CombatActionId.InstantHit;

        public void Execute(ICombatActionModel fireProjectileModel)
        {
            fireProjectileModel = (InstantHitModel)fireProjectileModel;
            
        }
    }
}