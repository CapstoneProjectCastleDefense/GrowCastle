namespace Models.Tags
{
    public class WeaknessTag : IEffectTag
    {
        public EffectTagEnum ElementEffectTagEnum => EffectTagEnum.WeaknessEffect;
        public float         StrengthReduction;
        public float         Duration;
        public float         Timer;
    }
}