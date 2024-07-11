namespace Models.Tags
{
    public class BleedTag : IEffectTag
    {
        public EffectTagEnum   ElementEffectTagEnum => EffectTagEnum.BleedAffect;
        public float Duration;
        public float TimeDelay;
        public float Timer;
    }
}