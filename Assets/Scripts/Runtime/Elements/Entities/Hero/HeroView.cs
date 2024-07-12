namespace Runtime.Elements.Entities.Hero
{
    using System;
    using Runtime.Elements.Base;
    using Spine.Unity;
    using UnityEngine;
    using UnityEngine.UI;

    public class HeroView : BaseCombatantView
    {
        public SkeletonAnimation skeletonAnimation;
        public Transform         spawnProjectilePos;
        public Action            OnClickAction;
        public Image             cooldownSkillBar;

        public void OnMouseDown()
        {
            this.OnClickAction?.Invoke();
        }
    }
}