namespace Models.Tags
{
    public interface IEffectTag
    {
        public EffectTagEnum ElementEffectTagEnum { get; }
    }

    public enum EffectTagEnum
    {
        BleedEffect,
        StunEffect,
        FearEffect,
        InstantDamageEffect,
        SlowEffect
    }
}