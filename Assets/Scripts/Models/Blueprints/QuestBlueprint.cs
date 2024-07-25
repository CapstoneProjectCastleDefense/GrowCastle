namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using Models.LocalData;
    using Runtime.Enums;

    [BlueprintReader("Quest", true)]
    [CsvHeaderKey("QuestId")]
    public class QuestBlueprint : GenericBlueprintReaderByRow<string,QuestRecord>
    {
    }

    public class QuestRecord
    {
        public string       QuestId         { get; set; }
        public string       Description     { get; set; }
        public float        TargetValue     { get; set; }
        public float       RewardValue     { get; set; }
        public ResourceType RewardType      { get; set; }
        public QuestType    QuestType       { get; set; }
        public string       SignalTriggerId { get; set; }
        public string       QuestIcon       { get; set; }
    }

    public enum QuestType
    {
        Daily,
        Weekly,
        Achievement,
    }
}