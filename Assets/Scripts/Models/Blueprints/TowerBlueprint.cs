
namespace Models.Blueprints
{
    using System.Collections.Generic;
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("Tower", true)]
    [CsvHeaderKey("TowerId")]
    public class TowerBlueprint : GenericBlueprintReaderByRow<string, TowerRecord>
    {
    }

    public class TowerRecord
    {
        public string TowerId { get; set; }
        public string PrefabName { get; set; }
        public string EvolutionId { get; set; }
        public float BaseATK { get; set; }
        public float ATKSPD { get; set; }
        public BlueprintByRow<string, SkillToAnimationRecord> SkillToAnimationRecords { get; set; }
    }

}
