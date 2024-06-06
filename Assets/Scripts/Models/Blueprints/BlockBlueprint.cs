namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("Block", true)] [CsvHeaderKey("Id")]
    public class BlockBlueprint : GenericBlueprintReaderByRow<string,BlockRecord>
    {
        
    }

    public class BlockRecord
    {
        public string                                  Id                  { get; set; }
        public BlueprintByRow<int, BlockToLevelRecord> BlockToLevelRecords { get; set; }
    }

    public class BlockToLevelRecord
    {
        public int    BlockLevel { get; set; }
        public string Image      { get; set; }
    }
}