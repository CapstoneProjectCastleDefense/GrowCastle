namespace Runtime.Combat.CombatActions.Actions
{
    using Runtime.Combat.CombatActions.Models;

    public interface ICombatAction
    {
        string ActionId { get; set; }
        void   Execute<TModel>(TModel model) where TModel : ICombatActionModel;
    }
    
    public interface ICombatAction<TModel> where TModel : ICombatActionModel
    {
    }
}