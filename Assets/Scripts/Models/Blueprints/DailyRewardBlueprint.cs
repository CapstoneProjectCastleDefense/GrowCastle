namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [CsvHeaderKey("Day")] [BlueprintReader("DailyReward", true)]
    public class DailyRewardBlueprint : GenericBlueprintReaderByRow<int, DailyRewardRecord>
    {
    }

    public class DailyRewardRecord
    {
        public int        Day         { get; set; }
        public string     RewardId    { get; set; }
        public int        RewardValue { get; set; }
        public string     RewardImage { get; set; }
        public RewardType RewardType  { get; set; }
    }

    public enum RewardType
    {
        Resource,
        Item,
        Chest,
        Character,
        Ticket,
    }

}