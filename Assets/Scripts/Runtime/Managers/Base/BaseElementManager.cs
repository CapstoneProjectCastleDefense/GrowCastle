namespace Runtime.Managers.Base
{
    using System.Collections.Generic;
    using Runtime.Elements.Base;
    using Zenject;

    public abstract class BaseElementManager<TModel, TPresenter, TView> : IElementManager, ITickable
        where TView : BaseElementView
        where TPresenter : BaseElementPresenter<TModel, TView, TPresenter>
        where TModel : IElementModel
    {
        public    List<TPresenter>                      entities = new();
        protected BaseElementPresenter<TModel, TView, TPresenter>.Factory Factory { get; }
        protected BaseElementManager(BaseElementPresenter<TModel, TView, TPresenter>.Factory factory)
        {
            this.Factory = factory;
        }
        public abstract void Initialize();
        public virtual TPresenter CreateElement(TModel model)
        {
            var presenter = this.Factory.Create(model);
            this.entities.Add(presenter);
            return presenter;
        }
        public abstract void           DisposeAllElement();
        public          IEnumerable<T> GetAllElementPresenter<T>() { return this.entities as IEnumerable<T>;}
        public virtual void Tick()
        {
            this.entities.ForEach(e=>e.Tick());
        }
    }
}