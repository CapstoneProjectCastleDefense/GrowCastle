namespace Runtime.Elements.Base
{
    using UnityEngine;

    public abstract class BaseElementView : MonoBehaviour
    {
        public LayerMask         LayerMask => this.gameObject.layer;
        public IElementPresenter Presenter { get; set; }
    }
}