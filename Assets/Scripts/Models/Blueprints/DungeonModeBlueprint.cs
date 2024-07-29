using BlueprintFlow.BlueprintReader;

namespace Models.Blueprints
{
    [CsvHeaderKey("Id")] [BlueprintReader("DungeonMode", true)]
    public class DungeonModeBlueprint : GenericBlueprintReaderByRow<string, DungeonModeRecord>
    {
    }

    public class DungeonModeRecord
    {
        public string Id { get; set; }
        public string EnvironmentId { get; set; }
        public string BossId { get; set; }
        public int Ticket { get; set; }
        public int RequireLevel { get; set; }
        public BlueprintByRow<DungeonWaveRecord> DungeonWaveRecord { get; set; }
        public BlueprintByRow<DungeonRewardRecord> DungeonRewardRecord { get; set; }
    }

    [CsvHeaderKey("WaveId")]
    public class DungeonWaveRecord 
    {
        public int WaveId { get; set; }
        public float Delay { get; set; }
    }

    [CsvHeaderKey("RewardType")]
    public class DungeonRewardRecord 
    {
        public ChestType RewardType { get; set; }
        public int Value { get; set; }
    }
}