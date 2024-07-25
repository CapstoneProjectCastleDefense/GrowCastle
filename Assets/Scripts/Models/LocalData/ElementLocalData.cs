namespace Models.LocalData
{
    using System;
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;

    public class ElementLocalData : ILocalDataHaveController<ElementLocalDataController>
    {
        public Dictionary<string, EvolutionElementData> ElementIdToEvolveData = new();
        
        public void Init()
        {
            this.ElementIdToEvolveData = new Dictionary<string, EvolutionElementData>();
        }
    }

    public class EvolutionElementData
    {
        public string ElementId   { get; set; }
        public string EvolutionId { get; set; }
    }
}