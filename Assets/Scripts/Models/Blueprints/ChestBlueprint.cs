namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using Models.LocalData;
    using Runtime.Enums;

    [BlueprintReader("Chest", true)] [CsvHeaderKey("ChestType")]
    public class ChestBlueprint : GenericBlueprintReaderByRow<ResourceType, ChestRecord>
    {
    }

    public class ChestRecord
    {
        public ResourceType             ChestType    { get; set; }
        public int                      ItemQuantity { get; set; }
        public BlueprintByRow<PoolItem> PoolItems    { get; set; }
        public string                   ChestIcon    { get; set; }
        public string                   ChestName    { get; set; }
    }

    public class PoolItem
    {
        public string ItemId { get; set; }
        public int    Value    { get; set; }
        public float  Weight   { get; set; }
    }
}