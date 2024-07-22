namespace Models.Tags
{
    public class IncreaseBasicAttackTag : IEffectTag
    {
        public EffectTagEnum ElementEffectTagEnum => EffectTagEnum.IncreaseBasicAttack;
        public float         IncreaseValue;
    }
}