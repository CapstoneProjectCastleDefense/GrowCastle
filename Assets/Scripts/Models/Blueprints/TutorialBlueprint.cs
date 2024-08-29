namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("Tutorial", true)] [CsvHeaderKey("TutId")]
    public class TutorialBlueprint : GenericBlueprintReaderByRow<string,TutorialRecord>
    {

    }

    public class TutorialRecord
    {
        public string TutId                      { get; set; }
        public string Description                { get; set; }
        public string ActiveButtonPath           { get; set; }
        public string TutNeedCompleteToTriggerId { get; set; }
    }
}