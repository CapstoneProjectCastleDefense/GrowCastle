namespace Runtime.Elements.Base
{
    using Runtime.BasePoolAbleItem;
    using Zenject;

    public class PoolableItemFactory
    {
        private readonly DiContainer diContainer;

        public PoolableItemFactory(DiContainer diContainer) { this.diContainer = diContainer; }

        public TController Create<TController, TModel>(TModel model)
            where TController : IPoolableItemPresenter
            where TModel : IPoolAbleItemModel
        {
            var controller = this.diContainer.Instantiate<TController>(new object[] { model });
            controller.Initialize();
            return controller;
        }
    }
}