namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models.Blueprints;
    using Runtime.Enums;
    using UnityEngine;

    public class ElementEvolutionLocalDataController : ILocalDataController
    {
        private readonly ElementEvolutionLocalData   elementEvolutionLocalData;
        private readonly EvolutionBlueprint          evolutionBlueprint;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly EvolutionInfoBlueprint      evolutionInfoBlueprint;

        public ElementEvolutionLocalDataController(
            ElementEvolutionLocalData elementEvolutionLocalData,
            EvolutionBlueprint evolutionBlueprint,
            ResourceLocalDataController resourceLocalDataController,
            EvolutionInfoBlueprint evolutionInfoBlueprint)
        {
            this.elementEvolutionLocalData   = elementEvolutionLocalData;
            this.evolutionBlueprint          = evolutionBlueprint;
            this.resourceLocalDataController = resourceLocalDataController;
            this.evolutionInfoBlueprint      = evolutionInfoBlueprint;
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

        public bool UpdateEvolutionId(string elementId, string evolutionId)
        {
            if (!this.elementEvolutionLocalData.ElementIdToEvolveData.TryGetValue(elementId, out var evolutionElementData))
            {
                throw new Exception($"Invalid element id: {elementId}");
            }

            if (!evolutionElementData.OwnedEvolutions.Contains(evolutionId))
            {
                if (this.resourceLocalDataController.SpendResource(ResourceType.Diamond, this.evolutionInfoBlueprint.GetDataById(evolutionId).Price))
                {
                    evolutionElementData.EvolutionId = evolutionId;
                    evolutionElementData.OwnedEvolutions.Add(evolutionId);
                    return true;
                }
                
            }
            else
            {
                evolutionElementData.EvolutionId = evolutionId;
                return true;
            }

            return false;
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