namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("CastleConfig", true)] [CsvHeaderKey("CastleConfigId")]
    public class CastleConfigBlueprint : GenericBlueprintReaderByRow<string,CastleConfigRecord>
    {
       
    }

    public class CastleConfigRecord
    {
        public string CastleConfigId        { get; set; }
        public int    BaseMP                { get; set; }
        public int    BaseHP                { get; set; }
        public int    BaseGoldNeedToUpgrade { get; set; }
        public int    CoefficientMP         { get; set; }
        public float  CoefficientHP         { get; set; }
        public float  CoefficientGold       { get; set; }
    }
}