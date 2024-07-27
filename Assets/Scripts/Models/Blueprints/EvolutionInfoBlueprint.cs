namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("EvolutionInfoBlueprint", true)]
    [CsvHeaderKey("EvolutionId")]
    public class EvolutionInfoBlueprint : GenericBlueprintReaderByRow<string, EvolutionInfoRecord>
    {
    }

    public class EvolutionInfoRecord
    {
        public string                                EvolutionId          { get; set; }
        public string                                EvolutionDescription { get; set; }
        public BlueprintByRow<string, AbilityRecord> AbilityRecords       { get; set; }
    }

    [CsvHeaderKey("AbilityId")]
    public class AbilityRecord
    {
        public string AbilityId          { get; set; }
        public string AbilityIcon        { get; set; }
        public string AbilityDescription { get; set; }
    }
}