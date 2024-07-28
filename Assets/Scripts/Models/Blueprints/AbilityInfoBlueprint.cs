namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("AbilityInfoBlueprint", true)]
    public class AbilityInfoBlueprint : GenericBlueprintReaderByRow<string, AbilityInfoRecord>
    {
        
    }


    [CsvHeaderKey("AbilityId")]
    public class AbilityInfoRecord
    {
        public string AbilityId          { get; set; }
        public string AbilityIcon        { get; set; }
        public string AbilityDescription { get; set; }
    }
}