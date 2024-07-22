namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("Feature", true)] [CsvHeaderKey("FeatureName")]
    public class FeatureBlueprint : GenericBlueprintReaderByRow<FeatureName, FeatureRecord>
    {
    }

    public class FeatureRecord
    {
        public FeatureName FeatureName         { get; set; }
        public string      Description         { get; set; }
        public int         TargetValueToUnlock { get; set; }
    }

    public enum FeatureName
    {
        Quest,
        Talent,
        Inventory
    }
}