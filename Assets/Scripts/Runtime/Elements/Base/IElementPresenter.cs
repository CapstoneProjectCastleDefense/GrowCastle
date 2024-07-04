namespace Runtime.Elements.Base
{
    using System;
    using Cysharp.Threading.Tasks;

    public interface IElementPresenter
    {
        void    Initialize();
        void    Dispose();
        UniTask UpdateView();


        BaseElementView GetView();
    }
}