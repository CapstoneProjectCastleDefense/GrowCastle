namespace Helpers
{
    using System;
    using UnityEngine;

    [RequireComponent(typeof(Collider2D))]
    public class ObjectCollideEventHelper : MonoBehaviour
    {
        public  Action<GameObject,GameObject> OnColliderTriggerEnter;

        private void OnTriggerEnter2D(Collider2D col)
        {
            this.OnColliderTriggerEnter?.Invoke(col.gameObject,this.gameObject);
        }
    }
}