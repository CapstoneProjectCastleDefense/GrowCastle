namespace Runtime.Elements.EntitySkillEffect
{
    using System;
    using System.Collections.Generic;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;

    public class ImmediateDamageEntitySkillEffect : IEntitySkillEffect
    {
        public EntitySkillEffectType EntitySkillEffectType { get; set; } = EntitySkillEffectType.ImmediateDamage;

        public void Execute(ITargetable target, Dictionary<StatEnum, (Type, object)> stats)
        {
            var targetStats = target.GetStats();
            var damage      = this.GetBaseDamage(stats) * (1 + this.GetCriticalDamage(stats));
            var targetHp    = targetStats.GetStat<float>(StatEnum.Health);
            targetHp -= damage;
            targetStats.SetStat(StatEnum.Health, targetHp);
        }

        private float GetBaseDamage(Dictionary<StatEnum, (Type, object)> stats) { return stats.GetStat<float>(StatEnum.Attack); }

        private float GetCriticalDamage(Dictionary<StatEnum, (Type, object)> stats) { return stats.GetStat<float>(StatEnum.CritDamage); }

        public void Tick(float deltaTime) { }
    }
}