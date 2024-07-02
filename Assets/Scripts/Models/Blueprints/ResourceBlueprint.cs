namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using Models.LocalData;

    [BlueprintReader("Resource", true)]
    [CsvHeaderKey("ResourceType")]
    public class ResourceBlueprint : GenericBlueprintReaderByRow<ResourceType,ResourceRecord>
    {

    }

    public class ResourceRecord
    {
        public ResourceType ResourceType { get; set; }
        public string       Image        { get; set; }
    }
}