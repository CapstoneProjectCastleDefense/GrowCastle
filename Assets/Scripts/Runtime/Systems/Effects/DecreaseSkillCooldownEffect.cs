namespace Runtime.Systems.Effects
{
    using System;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;

    public class DecreaseSkillCooldownEffect : BaseEffect
    {
        protected override void Filter()
        {
            
        }
        protected override void ActiveEffect(ITargetable target)
        {
            
        }
        public override    Type EffectTagType                               => typeof(DecreaseSkillCooldownTag);
        public override void Execute(ITargetable target, IEffectTag tag)
        {
            var targetStat  = target.GetStats();
            var targetCooldown = targetStat.GetStat<float>(StatEnum.ActiveSkillCooldown);
            targetCooldown -= targetCooldown * 0.5f;
            targetStat.SetStat(StatEnum.ActiveSkillCooldown, targetCooldown);
            target.UpdateStats();
        }
    }
}