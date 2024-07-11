namespace Models.Tags
{
    public class SlowTag : IEffectTag
    {
        public EffectTagEnum ElementEffectTagEnum => EffectTagEnum.SlowEffect;
        public float         Duration;
        public float         Timer;
        public float         InitialSpeed;
    }
}