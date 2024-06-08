namespace Runtime.Elements.Base
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using UnityEngine;
    using Zenject;

    public abstract class BaseElementPresenter<TModel, TView, TPresenter> : IInitializable, IElementPresenter, IDisposable
        where TView : BaseElementView where TPresenter : BaseElementPresenter<TModel, TView, TPresenter> where TModel : IElementModel
    {
        protected ObjectPoolManager ObjectPoolManager;

        protected BaseElementPresenter(TModel model, ObjectPoolManager objectPoolManager)
        {
            this.Model             = model;
            this.ObjectPoolManager = objectPoolManager;
        }

        public TModel Model { get; }
        public TView  View  { get; set; }

        public GameObject GetViewObject() => this.View.gameObject;

        public abstract void OnDestroyPresenter();

        public virtual async void Initialize()
        {
            this.UpdateView();
        }

        protected virtual async void UpdateView()
        {
            if (this.View != null) return;
            await this.CreateView().ContinueWith((viewObject) =>
            {
                this.View = viewObject.GetComponent<TView>();
            });
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