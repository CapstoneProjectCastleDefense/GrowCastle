namespace Runtime.Elements.Entities.Hero
{
    using System;
    using Runtime.Elements.Base;
    using Spine.Unity;
    using UnityEngine;

    public class HeroView : BaseElementView
    {
        public SkeletonAnimation skeletonAnimation;
        public Transform         spawnProjectilePos;
        public Action            OnClickAction;

        public void OnMouseDown()
        {
            this.OnClickAction?.Invoke();
        }
    }
}