namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ElementEvolutionLocalDataController : ILocalDataController
    {
        private readonly ElementEvolutionLocalData   elementEvolutionLocalData;
        private readonly EvolutionBlueprint evolutionBlueprint;

        public ElementEvolutionLocalDataController(ElementEvolutionLocalData elementEvolutionLocalData,
            EvolutionBlueprint evolutionBlueprint)
        {
            this.elementEvolutionLocalData   = elementEvolutionLocalData;
            this.evolutionBlueprint = evolutionBlueprint;
        }

        public void InitData()
        {
            if (this.elementEvolutionLocalData.ElementIdToEvolveData == null || this.elementEvolutionLocalData.ElementIdToEvolveData.Count == 0)
            {
                foreach (var (key, record) in this.evolutionBlueprint)
                {
                    var firstEvolutionId = record.LevelToEvolutionDetailRecords[1].EvolutionDetailRecords.First().Key;
                    this.elementEvolutionLocalData.ElementIdToEvolveData!.Add(key, new EvolutionElementData()
                    {
                        ElementId       = key,
                        EvolutionId     = firstEvolutionId,
                        OwnedEvolutions = new List<string> { firstEvolutionId }
                    });
                }
            }
        }

        public EvolutionElementData GetEvolutionElementData(string id)
        {
            if (!this.elementEvolutionLocalData.ElementIdToEvolveData.TryGetValue(id, out var evolutionElementData))
            {
                throw new Exception($"Invalid element id: {id}");
            }

            return evolutionElementData;
        }

        public void UpdateEvolutionId(string elementId, string evolutionId)
        {
            if (!this.elementEvolutionLocalData.ElementIdToEvolveData.TryGetValue(elementId, out var evolutionElementData))
            {
                throw new Exception($"Invalid element id: {elementId}");
            }

            evolutionElementData.EvolutionId = evolutionId;
            if (!evolutionElementData.OwnedEvolutions.Contains(evolutionId))
            {
                evolutionElementData.OwnedEvolutions.Add(evolutionId);
            }
        }

        public bool IsEvolutionUnlock(string elementId, string evolutionId)
        {
            if (!this.elementEvolutionLocalData.ElementIdToEvolveData.TryGetValue(elementId, out var evolutionElementData))
            {
                throw new Exception($"Invalid element id: {elementId}");
            }

            return evolutionElementData.OwnedEvolutions.Contains(evolutionId);
        }
    }
}