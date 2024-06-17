namespace Runtime.Elements.Base
{
    using System;
    using UnityEngine;

    public abstract class BaseElementView : MonoBehaviour
    {
        public LayerMask LayerMask => this.gameObject.layer;
        public string    Tag       => this.gameObject.tag;
        public IElementPresenter Presenter { get; set; }
    }
}