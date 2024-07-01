namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using Models.LocalData;

    [BlueprintReader("HeroConfig", true)] [CsvHeaderKey("HeroId")]
    public class HeroConfigBlueprint : GenericBlueprintReaderByRow<string, HeroConfigRecord>
    {
    }

    public class HeroConfigRecord
    {
        public string                                   HeroId               { get; set; }
        public float                                    BaseAttack           { get; set; }
        public float                                    BaseAttackSpeed      { get; set; }
        public float                                    BaseResource         { get; set; }
        public ResourceType                             ResourceType         { get; set; }
        public BlueprintByRow<int, LevelToConfigRecord> LevelToConfigRecords { get; set; }
    }

    [CsvHeaderKey("Level")]
    public class LevelToConfigRecord
    {
        public int    Level       { get; set; }
        public string Avatar      { get; set; }
        public float  Coefficient { get; set; }
    }
}