namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("Level", true)] [CsvHeaderKey("Id")]
    public class LevelBlueprint : GenericBlueprintReaderByRow<int, LevelRecord>
    {
    }

    public class LevelRecord
    {
        public int                                       Id                 { get; set; }
        public string                                    PrefabName         { get; set; }
        public BlueprintByRow<int, LevelToWaveRecord> LevelToWaveRecords { get; set; }
    }

    [CsvHeaderKey("WaveId")] public class LevelToWaveRecord
    {
        public int    WaveId        { get; set; }
        public float  Delay         { get; set; }
        public string EnvironmentId { get; set; }
    }
}