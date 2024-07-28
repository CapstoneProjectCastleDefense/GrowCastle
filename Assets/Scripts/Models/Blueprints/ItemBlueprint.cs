namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using Runtime.Enums;

    [BlueprintReader("Item", true)]
    [CsvHeaderKey("Id")]
    public class ItemBlueprint : GenericBlueprintReaderByRow<string, ItemRecord>
    {
    }

    public class ItemRecord
    {
        public string        Id            { get; set; }
        public string        Name          { get; set; }
        public string        Description   { get; set; }
        public string        ImageAddress  { get; set; }
        public ItemType      ItemType      { get; set; }
        public EquipmentType EquipmentType { get; set; }
        public int           MaxLevel      { get; set; }
        public int           MaxTier       { get; set; }
    }
}