namespace Runtime.Enums
{
    public enum StatEnum
    {
        Health,
        Mana,

        MaxHealth,
        MaxHealthBonus,
        MaxMana,
        MaxManaBonus,
        BonusReduceMana,
        ActiveSkillCooldown,
        ActiveSkillCooldownBonus,
        MaxSpeed,
        MaxSpeedBonus,
        MaxAttack,
        MaxExistTime,
        MaxExistTimeBonus,

        Attack,
        AttackBonus,
        Defense,
        DefenseBonus,
        AttackSpeed,
        AttackSpeedBonus,
        AttackRange,
        AttackRangeBonus,
        MoveSpeed,
        MoveSpeedBonus,
        CastSpeed,
        CastSpeedBonus,
        CritChance,
        CritChanceBonus,
        CritDamage,
        CritDamageBonus,
        ManaCost,
        ManaCostBonus,
        ExistTime,

        Gold,
        Exp,

        AttackPriority,

        TargetThatImAttacking,
        TargetThatImLookingAt,
        TargetThatAttackingMe,
        None
    }

    public enum StatType
    {
        Additive,
        Bonus,
        Percentage,
        Misc,
    }

    public static class StatExtension
    {
        public static StatType GetStatType(this StatEnum statEnum)
        {
            return statEnum switch
            {
                StatEnum.Health => StatType.Additive,
                StatEnum.Mana => StatType.Additive,
                StatEnum.MaxHealth => StatType.Additive,
                StatEnum.MaxHealthBonus => StatType.Bonus,
                StatEnum.MaxMana => StatType.Additive,
                StatEnum.MaxManaBonus => StatType.Bonus,
                StatEnum.BonusReduceMana => StatType.Bonus,
                StatEnum.ActiveSkillCooldown => StatType.Additive,
                StatEnum.ActiveSkillCooldownBonus => StatType.Bonus,
                StatEnum.MaxSpeed => StatType.Additive,
                StatEnum.MaxSpeedBonus => StatType.Bonus,
                StatEnum.MaxAttack => StatType.Additive,
                StatEnum.MaxExistTime => StatType.Additive,
                StatEnum.MaxExistTimeBonus => StatType.Bonus,
                StatEnum.Attack => StatType.Additive,
                StatEnum.AttackBonus => StatType.Bonus,
                StatEnum.Defense => StatType.Additive,
                StatEnum.DefenseBonus => StatType.Bonus,
                StatEnum.AttackSpeed => StatType.Additive,
                StatEnum.AttackSpeedBonus => StatType.Bonus,
                StatEnum.AttackRange => StatType.Additive,
                StatEnum.AttackRangeBonus => StatType.Bonus,
                StatEnum.MoveSpeed => StatType.Additive,
                StatEnum.MoveSpeedBonus => StatType.Bonus,
                StatEnum.CastSpeed => StatType.Additive,
                StatEnum.CastSpeedBonus => StatType.Bonus,
                StatEnum.CritChance => StatType.Percentage,
                StatEnum.CritChanceBonus => StatType.Bonus,
                StatEnum.CritDamage => StatType.Percentage,
                StatEnum.CritDamageBonus => StatType.Bonus,
                StatEnum.ManaCost => StatType.Additive,
                StatEnum.ManaCostBonus => StatType.Bonus,
                StatEnum.ExistTime => StatType.Additive,
                StatEnum.Gold => StatType.Misc,
                StatEnum.Exp => StatType.Misc,
                StatEnum.AttackPriority => StatType.Misc,
                StatEnum.TargetThatImAttacking => StatType.Misc,
                StatEnum.TargetThatImLookingAt => StatType.Misc,
                StatEnum.TargetThatAttackingMe => StatType.Misc,
                _ => StatType.Misc,
            };
        }
        public static StatEnum GetBonusStat(this StatEnum statEnum)
        {
            return statEnum switch
            {
                StatEnum.MaxHealth => StatEnum.MaxHealthBonus,
                StatEnum.MaxMana => StatEnum.MaxManaBonus,
                StatEnum.ActiveSkillCooldown => StatEnum.ActiveSkillCooldownBonus,
                StatEnum.MaxSpeed => StatEnum.MaxSpeedBonus,
                StatEnum.MaxExistTime => StatEnum.MaxExistTimeBonus,
                StatEnum.Attack => StatEnum.AttackBonus,
                StatEnum.Defense => StatEnum.DefenseBonus,
                StatEnum.AttackSpeed => StatEnum.AttackSpeedBonus,
                StatEnum.AttackRange => StatEnum.AttackRangeBonus,
                StatEnum.MoveSpeed => StatEnum.MoveSpeedBonus,
                StatEnum.CastSpeed => StatEnum.CastSpeedBonus,
                StatEnum.CritChance => StatEnum.CritChanceBonus,
                StatEnum.CritDamage => StatEnum.CritDamageBonus,
                StatEnum.ManaCost => StatEnum.ManaCostBonus,
                _ => StatEnum.None,
            };
        }
    }
}