namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ElementLocalDataController : ILocalDataController
    {
        private readonly ElementLocalData   elementLocalData;
        private readonly EvolutionBlueprint evolutionBlueprint;

        public ElementLocalDataController(ElementLocalData elementLocalData,
            EvolutionBlueprint evolutionBlueprint)
        {
            this.elementLocalData   = elementLocalData;
            this.evolutionBlueprint = evolutionBlueprint;
        }

        public void InitData()
        {
            if (this.elementLocalData.ElementIdToEvolveData == null || this.elementLocalData.ElementIdToEvolveData.Count == 0)
            {
                foreach (var (key, record) in this.evolutionBlueprint)
                {
                    var firstEvolutionId = record.LevelToEvolutionDetailRecords[1].EvolutionDetailRecords.First().Key;
                    this.elementLocalData.ElementIdToEvolveData!.Add(key, new EvolutionElementData()
                    {
                        ElementId       = key,
                        EvolutionId     = firstEvolutionId,
                        OwnedEvolutions = new List<string> { key }
                    });
                }
            }
        }

        public EvolutionElementData GetEvolutionElementData(string id)
        {
            if (!this.elementLocalData.ElementIdToEvolveData.TryGetValue(id, out var evolutionElementData))
            {
                throw new Exception($"Invalid element id: {id}");
            }

            return evolutionElementData;
        }

        public void UpdateEvolutionId(string elementId, string evolutionId)
        {
            if (!this.elementLocalData.ElementIdToEvolveData.TryGetValue(elementId, out var evolutionElementData))
            {
                throw new Exception($"Invalid element id: {elementId}");
            }

            evolutionElementData.EvolutionId = evolutionId;
            if (!evolutionElementData.OwnedEvolutions.Contains(evolutionId))
            {
                evolutionElementData.OwnedEvolutions.Add(evolutionId);
            }
        }
    }
}