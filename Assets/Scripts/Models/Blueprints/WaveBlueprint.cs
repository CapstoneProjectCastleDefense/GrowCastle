namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("Wave", true)]
    [CsvHeaderKey("Id")]
    public class WaveBlueprint : GenericBlueprintReaderByRow<int, WaveRecord>
    {
    }

    public class WaveRecord
    {
        public int                                       Id          { get; set; }
        public BlueprintByRow<string, WaveToEnemyRecord> WaveToEnemy { get; set; }
    }

    [CsvHeaderKey("EnemyId")]
    public class WaveToEnemyRecord
    {
        public string EnemyId  { get; set; }
        public int    Quantity { get; set; }
        public float  Delay    { get; set; }
    }
}