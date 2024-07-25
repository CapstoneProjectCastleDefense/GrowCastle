namespace Runtime.Systems.Effects
{
    using System;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using UnityEngine;

    public class FearEffect : BaseEffect
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
            var tagData      = (FearTag)target.CurrentEffectTags[this.EffectTagType];
            this.GetCurrentContainer().Resolve<EffectManager>().AddEffectToTarget(target,new SlowTag(){Duration = 2});
            
            if (tagData.Timer >= tagData.Duration)
            {
                target.GetStats().SetStat(StatEnum.MoveSpeed, target.GetStats().GetStat<float>(StatEnum.MaxSpeed));
                tagData.Duration = 0;
            }

            tagData.Timer += Time.deltaTime;
        }
        public override Type EffectTagType => typeof(FearTag);
        public override void Execute(ITargetable target, IEffectTag tag)
        {
            
        }
    }
}