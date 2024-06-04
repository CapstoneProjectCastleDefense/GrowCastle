namespace Runtime.Elements.Base
{
    using System;
    using UnityEngine;

    public abstract class BaseElementView : MonoBehaviour
    {
        private new Collider2D        collider2D;
        private     Action<Collision> onCollisionEnterAction;
        public      IElementPresenter Presenter { get; set; }

        public virtual void Start() { this.collider2D = this.GetComponent<Collider2D>(); }

        private void OnCollisionEnter(Collision collision) { this.onCollisionEnterAction?.Invoke(collision); }
    }
}