namespace Models.Tags
{
    public interface IEffectTag
    {
        public EffectTagEnum ElementEffectTagEnum { get; }
    }

    public enum EffectTagEnum
    {
        BleedEffect,
        FreezeEffect,
        FearEffect,
        InstantDamageEffect,
        SlowEffect,
        IncreaseAttackSpeed,
        DecreaseSkillCooldown,
        IncreaseBasicAttack,
        DeathEffect,
        WeaknessEffect
        
    }
}