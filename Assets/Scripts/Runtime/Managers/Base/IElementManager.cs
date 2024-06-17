namespace Runtime.Managers.Base
{
    using System.Collections.Generic;
    using Zenject;

    public interface IElementManager: IInitializable
    {
        void DisposeAllElement();
        IEnumerable<T> GetAllElementPresenter<T>();
    }
}