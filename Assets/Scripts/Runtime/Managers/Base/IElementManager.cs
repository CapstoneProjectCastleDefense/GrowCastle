namespace Runtime.Managers.Base
{
    using Zenject;

    public interface IElementManager: IInitializable
    {
        void DisposeAllElement();
    }
}