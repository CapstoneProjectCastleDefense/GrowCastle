namespace Models
{
    using System.Collections.Generic;
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("EvolutionBlueprint", true)]
    [CsvHeaderKey("CharacterId")]
    public class EvolutionBlueprint : GenericBlueprintReaderByRow<string, EvolutionRecord>
    {
    }

    public class EvolutionRecord
    {
        public string                                            CharacterId;
        public BlueprintByRow<int, LevelToEvolutionDetailRecord> LevelToEvolutionDetailRecords;
    }

    [CsvHeaderKey("Level")]
    public class LevelToEvolutionDetailRecord
    {
        public int                                           Level;
        public BlueprintByRow<string, EvolutionDetailRecord> EvolutionDetailRecords;
    }

    [CsvHeaderKey("EvolutionId")]
    public class EvolutionDetailRecord
    {
        public string       EvolutionId;
        public int          LineIndex;
        public string       ParentId;
        public int          ParentPathIndex;
        public string       IconImg;
        public List<string> NormalAttackSkills;
        public List<string> PassiveSkills;
        public List<string> ActiveSkills;
        public int          Price;
    }
}