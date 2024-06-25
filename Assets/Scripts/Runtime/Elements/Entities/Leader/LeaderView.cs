namespace Runtime.Elements.Entities.Leader
{
    using Runtime.Elements.Base;
    using Spine.Unity;
    using UnityEngine;
    using UnityEngine.UI;

    public class LeaderView : BaseElementView
    {
        [field: SerializeField] public SkeletonAnimation SkeletonAnimation     { get; private set; }
        [field: SerializeField] public Collider2D        GraphicCollider2D     { get; private set; }
        [field: SerializeField] public Collider2D        AttackRangeCollider2D { get; private set; }
        [field: SerializeField] public Rigidbody2D       Rigidbody2D           { get; private set; }
        [field: SerializeField] public Image             HealthBar             { get; private set; }
        [field: SerializeField] public GameObject        HealthBarContainer    { get; private set; }
    }
}