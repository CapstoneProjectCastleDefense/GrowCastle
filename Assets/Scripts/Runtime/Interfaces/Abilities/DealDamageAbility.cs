namespace Runtime.Interfaces.Abilities
{
    using System;
    using System.Collections.Generic;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.StaticValues;

    public class DealDamageAbility : IAbility
    {
        public string Id { get; set; } = AbilityName.DealDamage;
        public void Execute(ITargetable target, Dictionary<StatEnum, (Type, object)> stats)
        {
            var targetStats = target.GetStats();
            var health      = targetStats.GetStat<float>(StatEnum.Health);
            var damage      = stats.GetStat<float>(StatEnum.Attack);
            if (health <= 0)
            {
                return;
            }

            health -= damage;
            
            targetStats.SetStat(StatEnum.Health, health);
            target.OnGetHit(0f);
        }
    }
}