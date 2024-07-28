namespace Models.Blueprints
{
    using System.Collections.Generic;
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("EvolutionInfoBlueprint", true)]
    [CsvHeaderKey("EvolutionId")]
    public class EvolutionInfoBlueprint : GenericBlueprintReaderByRow<string, EvolutionInfoRecord>
    {
    }

    public class EvolutionInfoRecord
    {
        public string       EvolutionId          { get; set; }
        public string       EvolutionDescription { get; set; }
        public List<string> Abilities            { get; set; }
    }
}