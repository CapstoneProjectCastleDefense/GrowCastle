namespace Models.Tags
{
    public class FreezeTag : IEffectTag
    {
        public EffectTagEnum ElementEffectTagEnum => EffectTagEnum.FreezeEffect;
        public float         Duration;
        public float         Timer;
        public float         InitialSpeed;
    }
}