namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("ElementUpgradeBlueprint", true)]
    [CsvHeaderKey("Id")]
    public class ElementUpgradeBlueprint : GenericBlueprintReaderByRow<string, ElementUpgradeRecord>
    {
    }

    public class ElementUpgradeRecord
    {
        public string Id                        { get; set; }
        public float  BaseCost                  { get; set; }
        public int    LevelIntervalToChangeCost { get; set; }
        public float  CostChangePerInterval     { get; set; }
        public float  BaseAttack                { get; set; }
        public float  AttackEnhancePerInterval  { get; set; }
        public int    MaxLevel                  { get; set; }
    }
}