namespace Models.Tags
{
    public interface IEffectTag
    {
        public EffectTagEnum ElementEffectTagEnum { get; }
    }

    public enum EffectTagEnum
    {
        BleedAffect,
        StunEffect,
        FearEffect,
        InstantDamageEffect,
    }
}