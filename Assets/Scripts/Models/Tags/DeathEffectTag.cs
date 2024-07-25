namespace Models.Tags
{
    public class DeathEffectTag : IEffectTag
    {
        public EffectTagEnum ElementEffectTagEnum => EffectTagEnum.DeathEffect;
        public float         HpPercentRemainToTrigger;
    }
}