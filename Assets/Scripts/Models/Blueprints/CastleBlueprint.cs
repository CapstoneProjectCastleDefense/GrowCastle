namespace Models.Blueprints
{
    using System.Collections.Generic;
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("Castle", true)] [CsvHeaderKey("Level")]
    public class CastleBlueprint : GenericBlueprintReaderByRow<int, CastleRecord>
    {
    }

    public class CastleRecord
    {
        public int          Level            { get; set; }
        public List<string> SlotUnlock       { get; set; }
        public List<string>       BlockUnlock    { get; set; }
        public int          BlockUnlockLevel { get; set; }
    }
}