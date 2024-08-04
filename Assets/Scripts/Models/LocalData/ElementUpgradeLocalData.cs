namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;

    public class ElementUpgradeLocalData : ILocalDataHaveController<ElementUpgradeLocalDataController>
    {
        public Dictionary<string, ElementUpgradeData> IdToElementUpgradeData { get; set; }

        public void Init()
        {
            this.IdToElementUpgradeData = new Dictionary<string, ElementUpgradeData>()
            {
                {
                    "Knight", new ElementUpgradeData()
                    {
                        ElementId = "Knight",
                        Level     = 1
                    }
                }
            };
        }
    }

    public class ElementUpgradeData
    {
        public string ElementId { get; set; }
        public int    Level     { get; set; }
    }
}