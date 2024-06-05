namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("Environment", true)] [CsvHeaderKey("Id")]
    public class EnvironmentBlueprint : GenericBlueprintReaderByRow<string,EnvironmentRecord>
    {
        
    }

    public class EnvironmentRecord
    {
        public string Id         { get; set; }
        public string PrefabName { get; set; }
        public string EffectId   { get; set; }
    }
}