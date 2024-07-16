namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using Models.LocalData;

    [BlueprintReader("Talent", true)] [CsvHeaderKey("TalentType")]
    public class TalentBlueprint : GenericBlueprintReaderByRow<TalentType,TalentRecord>
    {
        
    }

    public class TalentRecord
    {
        public TalentType                                  TalentType               { get; set; }
        public string                                      Icon                     { get; set; }
        public string                                      Description              { get; set; }
        public BlueprintByRow<int,TalentLevelToDataRecord> TalentLevelToDataRecords { get; set; }
    }

    [CsvHeaderKey("Level")]
    public class TalentLevelToDataRecord
    {
        public int   Level           { get; set; }
        public float EffectValue     { get; set; }
        public int   TalentPointNeed { get; set; }
    }
}