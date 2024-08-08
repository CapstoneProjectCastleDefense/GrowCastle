namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using Runtime.Enums;

    [BlueprintReader("Skill", true)]
    [CsvHeaderKey("Id")]
    public class SkillBlueprint : GenericBlueprintReaderByRow<string, SkillRecord>
    {
    }

    public class SkillRecord
    {
        public string    Id          { get; set; }
        public SkillType Type        { get; set; }
        public float     Mana        { get; set; }
        public float     Cooldown    { get; set; }
        public string    Description { get; set; }
        public string    Icon        { get; set; }
    }
}