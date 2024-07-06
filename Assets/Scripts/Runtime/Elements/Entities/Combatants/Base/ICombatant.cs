namespace Runtime.Elements.Base
{
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Interfaces;

    public interface ICombatant : IElementModel, IHaveStats
    {
    }

    public interface ICombatantPresenter : IElementPresenter
    {
    }


    public abstract class BaseCombatantPresenter<TModel, TView, TPresenter> : BaseElementPresenter<TModel, TView, TPresenter>, ICombatantPresenter
        where TModel : ICombatant
        where TView : BaseElementView
        where TPresenter : BaseCombatantPresenter<TModel, TView, TPresenter>
    {
        protected BaseCombatantPresenter(TModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager)
        {
        }
    }
}