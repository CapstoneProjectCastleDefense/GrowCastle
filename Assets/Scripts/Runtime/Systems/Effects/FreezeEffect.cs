namespace Runtime.Systems.Effects
{
    using System;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using UnityEngine;

    public class FreezeEffect : BaseEffect
    {
        public override Type EffectTagType => typeof(FreezeTag);

        public override void Execute(ITargetable target, IEffectTag tag) { this.AddEffectToTarget(target, tag); }

        protected override void Filter()
        {
            for (var index = 0; index < this.AffectedElements.Count; index++)
            {
                var target = this.AffectedElements[index];
                if (((FreezeTag)target.CurrentEffectTags[this.EffectTagType]).Duration <= 0)
                {
                    this.RemoveEffectOnTarget(target);
                }
            }
        }
        protected override void ActiveEffect(ITargetable target)
        {
            var tagData = (FreezeTag)target.CurrentEffectTags[this.EffectTagType];
            target.GetStats().SetStat(StatEnum.MoveSpeed, 0f);

            if (tagData.Timer >= tagData.Duration)
            {
                target.GetStats().SetStat(StatEnum.MoveSpeed, target.GetStats().GetStat<float>(StatEnum.MaxSpeed));
                tagData.Duration = 0;
            }

            tagData.Timer += Time.deltaTime;
        }
    }
}