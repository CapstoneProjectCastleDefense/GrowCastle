namespace Runtime.Elements.Entities.Hero
{
    using System;
    using Runtime.Elements.Base;
    using Spine.Unity;
    using UnityEngine;

    public class HeroView : BaseCombatantView
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