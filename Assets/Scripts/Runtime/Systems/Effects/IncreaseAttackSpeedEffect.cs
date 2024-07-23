namespace Runtime.Systems.Effects
{
    using System;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;

    public class IncreaseAttackSpeedEffect : BaseEffect
    {
        protected override void Filter()                         { }
        protected override void ActiveEffect(ITargetable target) { }
        public override    Type EffectTagType                    => typeof(IncreaseAttackSpeedTag);
        public override void Execute(ITargetable target, IEffectTag tag)
        {
            var targetStat  = target.GetStats();
            var targetSpeed = targetStat.GetStat<float>(StatEnum.AttackSpeed);
            targetSpeed += targetSpeed * 0.2f;
            targetStat.SetStat(StatEnum.AttackSpeed, targetSpeed);
            target.UpdateStats();
        }
    }
}