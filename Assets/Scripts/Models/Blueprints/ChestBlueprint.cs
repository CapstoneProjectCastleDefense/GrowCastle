namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using Models.LocalData;

    [BlueprintReader("Chest", true)] [CsvHeaderKey("ChestType")]
    public class ChestBlueprint : GenericBlueprintReaderByRow<ChestType,ChestRecord>
    {
    }

    public class ChestRecord
    {
        public ChestType                ChestType    { get; set; }
        public int                      ItemQuantity { get; set; }
        public BlueprintByRow<PoolItem> PoolItems    { get; set; }
        public string                   ChestIcon    { get; set; }
    }

    public enum ChestType
    {
        CommonChest,
        SliverChest,
        GoldenChest,
        DiamondChest
    }

    public class PoolItem
    {
        public ResourceType ItemType { get; set; }
        public int          Value    { get; set; }
        public float          Weight   { get; set; }
    }
}