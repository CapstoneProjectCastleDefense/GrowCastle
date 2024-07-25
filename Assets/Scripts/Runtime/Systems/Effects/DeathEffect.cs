namespace Runtime.Systems.Effects
{
    using System;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;

    public class DeathEffect : BaseEffect
    {
        protected override void Filter()
        {
            
        }
        protected override void ActiveEffect(ITargetable target)
        {
            
        }
        public override Type EffectTagType => typeof(DeathEffectTag);
        public override void Execute(ITargetable target, IEffectTag tag)
        {
            var tagData         = (DeathEffectTag)tag;
            var hpPercentRemain = (target.GetStats().GetStat<float>(StatEnum.Health) / target.GetStats().GetStat<float>(StatEnum.MaxHealth)) * 100;
            if (!(hpPercentRemain < tagData.HpPercentRemainToTrigger)) return;
            target.GetStats().SetStat(StatEnum.Health,0f);
            target.UpdateStats();
        }
    }
}