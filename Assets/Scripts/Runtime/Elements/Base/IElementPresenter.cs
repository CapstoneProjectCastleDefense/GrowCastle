namespace Runtime.Elements.Base
{
    using Cysharp.Threading.Tasks;

    public interface IElementPresenter
    {
        UniTaskVoid Initialize();
        void Dispose();
        void UpdateView();
    }
}