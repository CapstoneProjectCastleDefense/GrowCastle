namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using Runtime.Enums;

    [BlueprintReader("Equipment", true)]
    [CsvHeaderKey("EquipmentId")]
    public class EquipmentBlueprint : GenericBlueprintReaderByRow<string, EquipmentRecord>
    {
    }

    public class EquipmentRecord
    {
        public string        EquipmentId { get; set; }
        public string        PrefabName  { get; set; }
        public EquipmentType Type        { get; set; }
    }
}