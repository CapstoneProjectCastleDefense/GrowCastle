namespace Models.LocalData.LocalDataController
{
    using System;

    public class ElementLocalDataController : ILocalDataController
    {
        private readonly ElementLocalData elementLocalData;

        public ElementLocalDataController(ElementLocalData elementLocalData) { this.elementLocalData = elementLocalData; }

        public void InitData()
        {
            if (this.elementLocalData.ElementIdToEvolveData == null || this.elementLocalData.ElementIdToEvolveData.Count == 0)
            {
                this.elementLocalData.ElementIdToEvolveData!.Add("Knight", new EvolutionElementData()
                {
                    ElementId   = "Knight",
                    EvolutionId = "knight_evolve_1"
                });
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
        }
    }
}