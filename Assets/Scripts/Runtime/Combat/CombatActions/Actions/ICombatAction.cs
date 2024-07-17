namespace Runtime.Combat.CombatActions.Actions
{
    using Runtime.Combat.CombatActions.Models;

    public interface ICombatAction
    {
        string ActionId { get; set; }
        void   Execute(ICombatActionModel fireProjectileModel);
    }
}