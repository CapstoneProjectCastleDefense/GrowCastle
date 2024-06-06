namespace Runtime.BasePoolAbleItem
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using UnityEngine;

    public abstract class BaseGameElementPresenter<TModel, TView> : IGameElementPresenter
        where TView : BaseGameElementView
        where TModel : IGameElementModel
    {
        private readonly ObjectPoolManager objectPoolManager;

        protected BaseGameElementPresenter(TModel model,
            ObjectPoolManager objectPoolManager)
        {
            this.objectPoolManager = objectPoolManager;
            this.Model             = model;
        }

        public TModel Model { get; }
        public TView  View  { get; set; }

        public virtual void Initialize() { }

        public async UniTask InitViewAsync()
        {
            var viewObject = await this.CreateView();
            this.View = viewObject.GetComponent<TView>();
            await this.InitViewInternal();
        }

        private async UniTask<GameObject> CreateView() { return await this.objectPoolManager.Spawn(this.Model.AddressableName); }

        protected virtual UniTask InitViewInternal() { return UniTask.CompletedTask; }

        public UniTask UpdateView() { return UniTask.CompletedTask; }

        public virtual void Dispose() { }
    }
}