namespace Runtime.Elements.Base
{
    public interface IElementPresenter
    {
        void Initialize();
        void Dispose();
        void UpdateView();
    }
}