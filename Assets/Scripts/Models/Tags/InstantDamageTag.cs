namespace Models.Tags
{
    public class InstantDamageTag : IEffectTag
    {
        public EffectTagEnum   ElementEffectTagEnum => EffectTagEnum.InstantDamageEffect;
        public float Damage;
    }
}