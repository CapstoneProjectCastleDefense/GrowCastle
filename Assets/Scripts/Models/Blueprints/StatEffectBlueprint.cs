namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("StatEffect", true)]
    [CsvHeaderKey("EffectId")]
    public class StatEffectBlueprint : GenericBlueprintReaderByRow<string, StatEffectRecord>
    {


    }

    public class StatEffectRecord
    {
        public string EffectId                  { get; set; }
        public float  AttackBonusPercent        { get; set; }
        public float  AttackSpeedBonusPercent   { get; set; }
        public float  SkillCooldownBonusPercent { get; set; }
    }
}