namespace Runtime.Elements.Base
{
    using System;
    using UnityEngine;

    public abstract class BaseElementView : MonoBehaviour
    {
        public IElementPresenter Presenter { get; set; }
    }
}