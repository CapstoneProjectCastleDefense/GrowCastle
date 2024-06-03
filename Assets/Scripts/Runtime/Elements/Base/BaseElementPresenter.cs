namespace Runtime.Elements.Base
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using TheOneStudio.HyperCasual.Runtime.Elements.Base;
    using UnityEngine;
    using Zenject;

    public abstract class BaseElementPresenter<TModel, TView, TPresenter> : IInitializable,IElementPresenter, IDisposable where TView : BaseElementView where TPresenter : BaseElementPresenter<TModel, TView, TPresenter>
    {
        protected ObjectPoolManager                             objectPoolManager;
        protected BaseElementPresenter(TModel model, ObjectPoolManager objectPoolManager)
        {
            this.Model             = model;
            this.objectPoolManager = objectPoolManager;
        }
        public TModel Model { get; }
        public TView  View  { get; set; }

        public          GameObject  GetViewObject() => this.View.gameObject;
        public abstract void        OnDestroyPresenter();

        public virtual async void Initialize()
        {
            this.UpdateView();
        }
        protected virtual async void UpdateView()
        {
            if (this.View == null)
            {
                var viewObject = await this.CreateView();
                this.View = viewObject.GetComponent<TView>();
            }
        }

        protected abstract UniTask<GameObject> CreateView();

        protected abstract void HandleCollisionEnter(Collision collision);

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