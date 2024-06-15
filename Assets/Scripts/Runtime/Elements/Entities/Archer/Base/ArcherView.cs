namespace Runtime.Elements.Entities.Archer.Base
{
    using Runtime.Elements.Base;
    using Spine.Unity;
    using UnityEngine;

    public class ArcherView : BaseElementView
    {
        public SkeletonAnimation skeletonAnimation;
        public GameObject        arrowPrefab;
        public Transform         spawnArrowPos;
    }
}