namespace Models.Tags
{
    public class IncreaseAttackSpeedTag : IEffectTag
    {
        public EffectTagEnum ElementEffectTagEnum => EffectTagEnum.IncreaseAttackSpeed;
        public float         IncreaseValue;
    }
}