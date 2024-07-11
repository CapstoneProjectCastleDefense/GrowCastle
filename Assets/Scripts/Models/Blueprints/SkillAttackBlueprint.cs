namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("SkillAttack", true)]
    [CsvHeaderKey("Id")]
    public class SkillAttackBlueprint : GenericBlueprintReaderByRow<string,SkillAttackRecord>
    {
        
    }

    public class SkillAttackRecord
    {
        public string                                  Id                   { get; set; }
        public BlueprintByRow<int,LevelToSkillAttack> LevelToConfigRecords { get; set; }
    }

    [CsvHeaderKey("Level")]
    public class LevelToSkillAttack
    {
        public int    Level      { get; set; }
        public string PrefabName { get; set; }
        public float  Damage     { get; set; }
    }
}