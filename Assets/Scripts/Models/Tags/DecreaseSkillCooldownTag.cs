namespace Models.Tags
{
    using Runtime.Systems.Effects;

    public class DecreaseSkillCooldownTag : IEffectTag
    {
        public EffectTagEnum ElementEffectTagEnum => EffectTagEnum.DecreaseSkillCooldown;
        public float         DecreaseValue;
    }
}