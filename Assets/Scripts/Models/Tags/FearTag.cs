namespace Models.Tags
{
    public class FearTag : IEffectTag
    {
        public EffectTagEnum ElementEffectTagEnum => EffectTagEnum.FearEffect;
        public float         Duration;
        public float         Timer;
    }
}