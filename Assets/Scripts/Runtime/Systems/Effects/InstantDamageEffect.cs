namespace Runtime.Systems.Effects
{
    using System;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;

    public class InstantDamageEffect : BaseEffect
    {
        protected override void Filter()
        {
            
        }
        protected override void ActiveEffect(ITargetable target)
        {
            
        }
        public override    Type EffectTagType                    { get; } = typeof(InstantDamageTag);
        public override void Execute(ITargetable target, IEffectTag tag)
        {
            var instantDamageTag = (InstantDamageTag)tag;
            var targetStat       = target.GetStats();
            var targetHp         = targetStat.GetStat<float>(StatEnum.Health);
            if (targetHp < 0) return;
            targetHp -= instantDamageTag.Damage;
            targetStat.SetStat(StatEnum.Health, targetHp);
            target.UpdateStats();
        }
    }
}