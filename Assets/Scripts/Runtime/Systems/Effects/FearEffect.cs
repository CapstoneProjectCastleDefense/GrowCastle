namespace Runtime.Systems.Effects
{
    using System;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Tags;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;

    public class FearEffect : BaseEffect
    {
        protected override void Filter()
        {
        }

        protected override void ActiveEffect(ITargetable target)
        {
        }

        public override Type EffectTagType => typeof(FearTag);

        public override void Execute(ITargetable target, IEffectTag tag)
        {
            this.AddEffectToTarget(target, tag);
            var tagData = (FearTag)target.CurrentEffectTags[this.EffectTagType];
            this.GetCurrentContainer().Resolve<EffectManager>().AddEffectToTarget(target, new SlowTag() { Duration     = tagData.Duration, Timer             = 0 });
            this.GetCurrentContainer().Resolve<EffectManager>().AddEffectToTarget(target, new WeaknessTag() { Duration = tagData.Duration, StrengthReduction = 0.9f, Timer = 0 });
        }
    }
}