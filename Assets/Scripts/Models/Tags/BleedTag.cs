namespace Models.Tags
{
    public class BleedTag : IEffectTag
    {
        public EffectTagEnum   ElementEffectTagEnum => EffectTagEnum.BleedEffect;
        public float Duration;
        public float TimeDelay;
        public float Timer;
    }
}