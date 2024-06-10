namespace Runtime.Elements.Base
{
    using Cysharp.Threading.Tasks;

    public interface IElementPresenter
    {
        void    Initialize();
        void    Dispose();
        UniTask UpdateView();
    }
}