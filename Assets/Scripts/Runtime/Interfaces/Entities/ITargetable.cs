namespace Runtime.Interfaces.Entities
{
    using UnityEngine;

    public interface ITargetable
    {
        LayerMask   LayerMask { get; }
        string      Tag       { get; }
        void        OnGetHit(float damage);
        void        OnDeath();
        T           GetModel<T>();
        T           GetView<T>();
        ITargetable TargetThatImAttacking { get; set; }
        ITargetable TargetThatAttackingMe { get; set; }
        bool        IsDead { get; }
    }
}