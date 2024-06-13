namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("Archer", true)] [CsvHeaderKey("Level")]
    public class ArcherBlueprint : GenericBlueprintReaderByRow<int,ArcherRecord>
    {

    }

    public class ArcherRecord
    {
        public int    Level      { get; set; }
        public string PrefabName { get; set; }
    }
}