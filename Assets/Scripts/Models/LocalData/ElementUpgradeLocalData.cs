namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;

    public class ElementUpgradeLocalData : ILocalDataHaveController<ElementUpgradeLocalDataController>
    {
        public Dictionary<string, ElementUpgradeData> IdToElementUpgradeData { get; set; }

        public void Init() { this.IdToElementUpgradeData = new Dictionary<string, ElementUpgradeData>(); }
    }

    public class ElementUpgradeData
    {
        public string ElementId { get; set; }
        public int    Level     { get; set; }
    }
}