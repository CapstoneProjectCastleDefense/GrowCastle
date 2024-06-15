namespace Runtime.Elements.Base
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using UnityEngine;
    using Zenject;

    public abstract class BaseElementPresenter<TModel, TView, TPresenter> : IElementPresenter
        where TView : BaseElementView where TPresenter : BaseElementPresenter<TModel, TView, TPresenter> where TModel : IElementModel
    {
        protected ObjectPoolManager ObjectPoolManager;

        protected BaseElementPresenter(TModel model, ObjectPoolManager objectPoolManager)
        {
            this.Model             = model;
            this.ObjectPoolManager = objectPoolManager;
        }

        protected TModel Model { get; }
        protected TView  View  { get; private set; }

        public virtual void Initialize() { }
        
        public virtual void Tick(){}

        public virtual async UniTask UpdateView()
        {
            if (this.View == null)
            {
                var viewObject = await this.CreateView();
                this.View = viewObject.GetComponent<TView>();
            }
            // then do something with the view
        }

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