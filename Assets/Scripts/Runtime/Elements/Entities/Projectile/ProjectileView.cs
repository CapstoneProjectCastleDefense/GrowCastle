namespace Runtime.Elements.Entities.Projectile
{
    using System;
    using Runtime.Elements.Base;
    using UnityEngine;

    public class ProjectileView : BaseElementView
    {
        public Action<Collider2D> ProjectileHit;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            Debug.Log("Projectile hit");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            this.ProjectileHit?.Invoke(other);
        }
    }
}