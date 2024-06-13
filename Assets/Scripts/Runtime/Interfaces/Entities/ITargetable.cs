namespace Runtime.Interfaces.Entities
{
    using UnityEngine;

    public interface ITargetable
    {
        LayerMask LayerMask { get; }
        string Tag { get; }
        void OnGetHit(float damage);
        void OnDeath();
    }
}