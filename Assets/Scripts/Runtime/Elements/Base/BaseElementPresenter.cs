namespace Runtime.Elements.Base
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Managers.Base;
    using UnityEngine;
    using Zenject;

    public abstract class BaseElementPresenter<TModel, TView, TPresenter> : IElementPresenter
        where TView : BaseElementView where TPresenter : BaseElementPresenter<TModel, TView, TPresenter> where TModel : IElementModel
    {
        protected readonly ObjectPoolManager                             ObjectPoolManager;
        protected          BaseElementManager<TModel, TPresenter, TView> ElementManager;
        protected BaseElementPresenter(TModel model, ObjectPoolManager objectPoolManager)
        {
            this.Model             = model;
            this.ObjectPoolManager = objectPoolManager;
        }

        protected      bool   IsViewInit                                                        { get; set; }
        protected      TModel Model                                                             { get; }
        protected      TView  View                                                              { get; private set; }
        public         void   SetManager(BaseElementManager<TModel, TPresenter, TView> manager) => this.ElementManager = manager;
        public virtual void   Initialize()                                                      { }
        public virtual void   Tick()                                                            { }

        public virtual async UniTask UpdateView()
        {
            if (!this.IsViewInit)
            {
                var viewObject = await this.CreateView();
                this.View       = viewObject.GetComponent<TView>();
                this.IsViewInit = true;
            }
        }

        public BaseElementView GetView()           => this.View;

        protected abstract UniTask<GameObject> CreateView();

        public class Factory : PlaceholderFactory<TModel, TPresenter>
        {
            public Factory(DiContainer container) { this.Container = container; }

            private DiContainer Container { get; }

            public override TPresenter Create(TModel param)
            {
                var presenter = this.Container.Instantiate<TPresenter>(new object[] { param });
                presenter.Initialize();

                return presenter;
            }
        }

        public abstract void Dispose();
    }
}