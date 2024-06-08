namespace Models.Blueprints {

    using BlueprintFlow.BlueprintReader;
    [BlueprintReader("Slot",true)]
    [CsvHeaderKey("Id")]
    public class SlotBlueprint : GenericBlueprintReaderByRow<string, SlotRecord>
    {

    }

    public class SlotRecord {
        public string Id { get; set; }
        public string SlotType { get; set; }
        public string Image { get; set; }
        public string EffectId { get; set; }
    }
}
