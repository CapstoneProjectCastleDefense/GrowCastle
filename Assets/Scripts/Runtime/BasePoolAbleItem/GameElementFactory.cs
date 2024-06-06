namespace Runtime.Elements.Base
{
    using Runtime.BasePoolAbleItem;
    using Zenject;

    public class GameElementFactory
    {
        private readonly DiContainer diContainer;

        public GameElementFactory(DiContainer diContainer) { this.diContainer = diContainer; }

        public TController Create<TController, TModel>(TModel model)
            where TController : IGameElementPresenter
            where TModel : IElementModel
        {
            var controller = this.diContainer.Instantiate<TController>(new object[] { model });
            controller.Initialize();
            return controller;
        }
    }
}