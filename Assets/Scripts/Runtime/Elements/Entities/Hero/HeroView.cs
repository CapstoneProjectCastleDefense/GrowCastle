namespace Runtime.Elements.Entities.Hero
{
    using System;
    using Runtime.Elements.Base;
    using Spine.Unity;

    public class HeroView : BaseElementView
    {
        public SkeletonAnimation skeletonAnimation;
        public Action            OnClickAction;

        public void OnMouseDown()
        {
            this.OnClickAction?.Invoke();
        }
    }
}