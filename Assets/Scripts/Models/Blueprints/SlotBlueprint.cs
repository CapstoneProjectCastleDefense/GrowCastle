namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using Models.LocalData;
    using UnityEngine;

    [BlueprintReader("Slot", true)] [CsvHeaderKey("Id")]
    public class SlotBlueprint : GenericBlueprintReaderByRow<int,SlotRecord>
    {
        
    }

    public class SlotRecord
    {
        public int      Id       { get; set; }
        public SlotType SlotType { get; set; }
        public string   Image    { get; set; }
        public string   EffectId { get; set; }
        public Vector3  Position { get; set; }
    }
}