namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using UnityEngine;

    [BlueprintReader("SkillSummon", true)]
    [CsvHeaderKey("Id")]
    public class SkillSummonBlueprint : GenericBlueprintReaderByRow<string, SkillSummonRecord>
    {
    }

    public class SkillSummonRecord
    {
        public string                                 Id                  { get; set; }
        public BlueprintByRow<int,SkillToLevelRecord> SkillToLevelRecords { get; set; }
    }

    [CsvHeaderKey("Level")]
    public class SkillToLevelRecord
    {
        public int     Level         { get; set; }
        public int     NumberSpawn   { get; set; }
        public string  PrefabName    { get; set; }
        public float   BaseHP        { get; set; }
        public float   BaseAttack    { get; set; }
        public float   TimeExist     { get; set; }
        public float   DistanceRange { get; set; }
        public Vector3 StartPos      { get; set; }
        public string  Description   { get; set; }
    }
}