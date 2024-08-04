namespace Models.LocalData.LocalDataController
{
    using System;
    using Models.Blueprints;

    public class ElementUpgradeLocalDataController : ILocalDataController
    {
        private readonly ElementUpgradeLocalData elementUpgradeLocalData;
        private readonly ElementUpgradeBlueprint elementUpgradeBlueprint;

        public ElementUpgradeLocalDataController(
            ElementUpgradeLocalData elementUpgradeLocalData,
            ElementUpgradeBlueprint elementUpgradeBlueprint)
        {
            this.elementUpgradeLocalData = elementUpgradeLocalData;
            this.elementUpgradeBlueprint = elementUpgradeBlueprint;
        }

        public void InitData()
        {
            if (this.elementUpgradeLocalData.IdToElementUpgradeData != null && this.elementUpgradeLocalData.IdToElementUpgradeData.Count != 0) return;
            
            foreach (var (key, _) in this.elementUpgradeBlueprint)
            {
                this.elementUpgradeLocalData.IdToElementUpgradeData!.Add(key, new ElementUpgradeData()
                {
                    ElementId = key,
                    Level     = 1
                });
            }
        }

        public ElementUpgradeData GetElementUpgradeData(string elementId)
        {
            if (!this.elementUpgradeLocalData.IdToElementUpgradeData.TryGetValue(elementId, out var elementUpgradeData))
            {
                throw new Exception($"Not found element upgrade data of element: {elementId}");
            }

            return elementUpgradeData;
        }

        public void UpgradeElement(string elementId, int levelUpgradeAmount = 1)
        {
            if (!this.elementUpgradeLocalData.IdToElementUpgradeData.TryGetValue(elementId, out var elementUpgradeData))
            {
                throw new Exception($"Not found element upgrade data of element: {elementId}");
            }

            elementUpgradeData.Level += levelUpgradeAmount;
        }
    }
}