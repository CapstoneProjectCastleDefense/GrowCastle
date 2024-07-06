namespace Runtime.Elements.Base
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using Models.Tags;

    public interface IElementPresenter
    {
        void                 Initialize();
        void                 Dispose();
        UniTask              UpdateView();
        BaseElementView GetView();
    }
}