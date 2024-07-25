namespace Runtime.Systems.Effects
{
    using System;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using UnityEngine;

    public class WeaknessEffect : BaseEffect
    {
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
            var tagData         = (WeaknessTag)target.CurrentEffectTags[this.EffectTagType];
            var currentStrength = target.GetStats().GetStat<float>(StatEnum.Attack);
            target.GetStats().SetStat(StatEnum.Attack, currentStrength * (1 - tagData.StrengthReduction));

            if (tagData.Timer >= tagData.Duration)
            {
                target.GetStats().SetStat(StatEnum.Attack, target.GetStats().GetStat<float>(StatEnum.MaxAttack));
                tagData.Duration = 0;
            }

            tagData.Timer += Time.deltaTime;
        }
        public override Type EffectTagType                               => typeof(WeaknessTag);
        public override void Execute(ITargetable target, IEffectTag tag) { this.AddEffectToTarget(target, tag); }
        protected override void AddEffectToTarget(ITargetable target, IEffectTag tag)
        {
            if (!target.CurrentEffectTags.ContainsKey(this.EffectTagType))
            {
                target.CurrentEffectTags.Add(tag.GetType(), tag);
                this.AffectedElements.Add(target);
                return;
            }

            ((WeaknessTag)target.CurrentEffectTags[tag.GetType()]).Duration += ((WeaknessTag)tag).Duration;
        }
    }
}