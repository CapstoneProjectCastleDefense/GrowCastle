namespace Runtime.Managers.Base
{
    using System.Collections.Generic;
    using Runtime.Elements.Base;

    public abstract class BaseElementManager<TModel, TPresenter, TView> : IElementManager where TView : BaseElementView where TPresenter : BaseElementPresenter<TModel, TView, TPresenter>
    {
        public    List<TPresenter>                      entities = new();
        protected BaseElementPresenter<TModel, TView, TPresenter>.Factory Factory { get; }
        protected BaseElementManager(BaseElementPresenter<TModel, TView, TPresenter>.Factory factory) { this.Factory = factory; }
        public abstract void       Initialize();
        public abstract TPresenter CreateElement(TModel model);
        public abstract void       DisposeAllElement();
    }
}