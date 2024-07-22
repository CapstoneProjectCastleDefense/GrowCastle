namespace Runtime.StaticValues
{
    using System;
    using System.Collections.Generic;
    using Models.Tags;

    public static class EffectIdToEffectType
    {
        public static Dictionary<string, Type> EffectIdToEffect = new()
        {
            { "DecreaseSkillCooldown", typeof(DecreaseSkillCooldownTag) },
            { "IncreaseAttackSpeed", typeof(IncreaseAttackSpeedTag) }
        };
    }
}