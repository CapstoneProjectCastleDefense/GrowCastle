namespace Runtime.Combat.CombatActions.Actions
{
    using Runtime.Combat.CombatActions.Models;

    public class FireProjectile : ICombatAction<FireProjectileModel>
    {
        public string ActionId { get; set; } = CombatActionId.FireProjectile;

        public void Execute(FireProjectileModel model)
        {
            
        }
    }
}