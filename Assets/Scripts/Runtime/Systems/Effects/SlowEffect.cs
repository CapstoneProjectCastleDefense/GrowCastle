namespace Runtime.Systems.Effects
{
    using System;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using UnityEngine;

    public class SlowEffect : BaseEffect
    {
        public override Type EffectTagType                               => typeof(SlowTag);
        public override void Execute(ITargetable target, IEffectTag tag) { this.AddEffectToTarget(target, tag); }

        protected override void AddEffectToTarget(ITargetable target, IEffectTag tag)
        {
            if (!target.CurrentEffectTags.ContainsKey(this.EffectTagType))
            {
                target.CurrentEffectTags.Add(tag.GetType(), tag);
                this.AffectedElements.Add(target);
                return;
            }

            ((SlowTag)target.CurrentEffectTags[tag.GetType()]).Duration+= ((SlowTag)tag).Duration;
        }

        protected override void Filter()
        {
            for (var index = 0; index < this.AffectedElements.Count; index++)
            {
                var target = this.AffectedElements[index];
                if (((SlowTag)target.CurrentEffectTags[this.EffectTagType]).Duration <= 0)
                {
                    this.RemoveEffectOnTarget(target);
                }
            }
        }
        protected override void ActiveEffect(ITargetable target)
        {
            var tagData = (SlowTag)target.CurrentEffectTags[this.EffectTagType];
            var speed   = target.GetStats().GetStat<float>(StatEnum.MaxSpeed) * 0.2f;
            target.GetStats().SetStat(StatEnum.MoveSpeed, speed);

            if (tagData.Timer >= tagData.Duration)
            {
                target.GetStats().SetStat(StatEnum.MoveSpeed,  target.GetStats().GetStat<float>(StatEnum.MaxSpeed));
                tagData.Duration = 0;
            }

            tagData.Timer += Time.deltaTime;
        }
    }
}