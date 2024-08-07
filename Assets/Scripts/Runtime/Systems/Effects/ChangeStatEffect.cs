namespace Runtime.Systems.Effects
{
    using System;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;

    public class ChangeStatEffect : BaseEffect
    {
        private readonly StatEffectBlueprint statEffectBlueprint;

        public ChangeStatEffect(StatEffectBlueprint statEffectBlueprint)
        {
            this.statEffectBlueprint = statEffectBlueprint;
        }

        protected override void Filter()
        {
        }

        protected override void ActiveEffect(ITargetable target)
        {
        }

        public override Type EffectTagType => typeof(ChangeStatTag);

        public override void Execute(ITargetable target, IEffectTag tag)
        {
            var tagData           = (ChangeStatTag)tag;
            var effectData        = this.statEffectBlueprint.GetDataById(tagData.EffectStatId);
            var currentTargetStat = target.GetStats();
            currentTargetStat.SetStat(StatEnum.Attack, currentTargetStat.GetStat<float>(StatEnum.Attack) * (1 + effectData.AttackBonusPercent / 100));
            currentTargetStat.SetStat(StatEnum.AttackSpeed, currentTargetStat.GetStat<float>(StatEnum.AttackSpeed) * (1 + effectData.AttackSpeedBonusPercent / 100));
            currentTargetStat.SetStat(StatEnum.ActiveSkillCooldown, currentTargetStat.GetStat<float>(StatEnum.ActiveSkillCooldown) * (1 - effectData.SkillCooldownBonusPercent / 100));
            target.UpdateStats();
        }
    }
}