namespace Models
{
    using System;
    using System.Collections.Generic;
    using BlueprintFlow.BlueprintReader;
    using Extensions;

    [BlueprintReader("EvolutionBlueprint", true)]
    [CsvHeaderKey("ElementId")]
    public class EvolutionBlueprint : GenericBlueprintReaderByRow<string, EvolutionRecord>
    {
        public EvolutionDetailRecord GetEvolutionDetailRecord(string elementId, string evolutionId)
        {
            var evolutionRecord = this.GetDataById(elementId);
            foreach (var (_, detailRecord) in evolutionRecord.LevelToEvolutionDetailRecords)
            {
                foreach (var record in detailRecord.EvolutionDetailRecords)
                {
                    if (record.Value.EvolutionId == evolutionId)
                    {
                        return record.Value;
                    }
                }
            }

            throw new Exception($"Does not have evolution id: {evolutionId}");
        }

        public List<string> GetPredecessorEvolveId(string elementId, string evolutionId)
        {
            var evolutionDetailRecord = this.GetEvolutionDetailRecord(elementId, evolutionId);
            var predecessorId         = evolutionDetailRecord.ParentId;
            var listPredecessor       = new List<string>();
            while (!predecessorId.IsNullOrEmpty())
            {
                listPredecessor.Add(predecessorId);
                var parentEvolutionDetailRecord = this.GetEvolutionDetailRecord(elementId, predecessorId);
                predecessorId = parentEvolutionDetailRecord.ParentId;
            }

            return listPredecessor;
        }

        public List<string> GetChildrenEvolutionId(string elementId, string evolutionId)
        {
            var evolutionRecord = this.GetDataById(elementId);
            var listChild       = new List<string>();
            foreach (var (_, detailRecord) in evolutionRecord.LevelToEvolutionDetailRecords)
            {
                foreach (var record in detailRecord.EvolutionDetailRecords)
                {
                    if (!record.Value.ParentId.IsNullOrEmpty() && record.Value.ParentId == evolutionId)
                    {
                        listChild.Add(record.Value.EvolutionId);
                    }
                }
            }

            return listChild;
        }
    }

    public class EvolutionRecord
    {
        public string                                            ElementId;
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