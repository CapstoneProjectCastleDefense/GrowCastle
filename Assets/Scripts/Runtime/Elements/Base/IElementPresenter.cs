﻿namespace TheOneStudio.HyperCasual.Runtime.Elements.Base
{
    using UnityEngine;

    public interface IElementPresenter
    {
        GameObject  GetViewObject();
        void        OnDestroyPresenter();
    }
}