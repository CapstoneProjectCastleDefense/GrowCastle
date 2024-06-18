namespace Runtime.Elements.Base
{
    using System;
    using Cysharp.Threading.Tasks;

    public interface IElementPresenter
    {
        void    Initialize();
        void    Dispose();
        UniTask UpdateView();

        T GetModelGeneric<T>();
        T GetViewGeneric<T>();

        IElementModel   GetModel();
        BaseElementView GetView();
    }
}