namespace Runtime.Managers.Base
{
    using System.Collections.Generic;
    using Runtime.Elements.Base;

    public abstract class BaseElementManager<TModel, TPresenter, TView> : IElementManager where TView : BaseElementView where TPresenter : BaseElementPresenter<TModel, TView, TPresenter> where TModel : IElementModel
    {
        public    List<TPresenter>                      entities = new();
        protected BaseElementPresenter<TModel, TView, TPresenter>.Factory Factory { get; }
        protected BaseElementManager(BaseElementPresenter<TModel, TView, TPresenter>.Factory factory) { this.Factory = factory; }
        public abstract void       Initialize();
        public virtual TPresenter CreateElement(TModel model)
        {
            var presenter = this.Factory.Create(model);
            this.entities.Add(presenter);
            return presenter;
        }
        public abstract void       DisposeAllElement();
    }
}