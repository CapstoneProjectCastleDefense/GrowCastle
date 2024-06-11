namespace Runtime.Elements.Entities.Enemy
{
    using Runtime.Elements.Base;
    using UnityEngine;

    public class BaseEnemyView : BaseElementView
    {
        [field: SerializeField] public SpriteRenderer SpriteRenderer        { get; private set; }
        [field: SerializeField] public Collider2D     GraphicCollider2D     { get; private set; }
        [field: SerializeField] public Collider2D     AttackRangeCollider2D { get; private set; }
        [field: SerializeField] public Rigidbody2D    Rigidbody2D           { get; private set; }
        [field: SerializeField] public Animator       Animator              { get; private set; }
    }
}