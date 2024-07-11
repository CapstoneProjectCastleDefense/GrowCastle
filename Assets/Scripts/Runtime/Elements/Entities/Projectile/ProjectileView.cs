namespace Runtime.Elements.Entities.Projectile
{
    using System;
    using Runtime.Elements.Base;
    using UnityEngine;

    public class ProjectileView : BaseElementView
    {
        [HideInInspector] public Action<Collider2D> projectileHitTrigger;
        private void OnTriggerEnter2D(Collider2D other) { this.projectileHitTrigger?.Invoke(other); }
    }
}