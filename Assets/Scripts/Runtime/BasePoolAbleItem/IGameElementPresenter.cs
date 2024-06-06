namespace Runtime.BasePoolAbleItem
{
    using Cysharp.Threading.Tasks;

    public interface IGameElementPresenter
    {
        void    Initialize();
        UniTask InitViewAsync();
        UniTask UpdateView();
        void    Dispose();
    }

    public interface IGameElementPresenter<TModel,TView> 
        where TModel : IGameElementModel
        where TView : IGameElementView
    {
        TModel Model { get; set; }
        TView  View  { get; set; }
    }
}