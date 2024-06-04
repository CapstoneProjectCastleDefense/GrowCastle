namespace Runtime.BasePoolAbleItem
{
    using Cysharp.Threading.Tasks;

    public interface IPoolableItemPresenter
    {
        void    Initialize();
        UniTask InitViewAsync();
        UniTask UpdateView();
        void    Dispose();
    }
}