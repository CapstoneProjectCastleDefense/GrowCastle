namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("CastleConfig", true)]
    [CsvHeaderKey("CastleConfigId")]
    public class CastleConfigBlueprint : GenericBlueprintReaderByRow<string, CastleConfigRecord>
    {
    }

    public class CastleConfigRecord
    {
        public string CastleConfigId        { get; set; }
        public float  BaseMP                { get; set; }
        public float  BaseHP                { get; set; }
        public float  BaseGoldNeedToUpgrade { get; set; }
        public float  CoefficientMP         { get; set; }
        public float  CoefficientHP         { get; set; }
        public float  CoefficientGold       { get; set; }
    }
}