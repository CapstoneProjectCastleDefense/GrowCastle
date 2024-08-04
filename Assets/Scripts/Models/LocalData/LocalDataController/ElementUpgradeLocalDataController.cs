namespace Models.LocalData.LocalDataController
{
    using System;

    public class ElementUpgradeLocalDataController : ILocalDataController
    {
        private readonly ElementUpgradeLocalData elementUpgradeLocalData;

        public ElementUpgradeLocalDataController(ElementUpgradeLocalData elementUpgradeLocalData) { this.elementUpgradeLocalData = elementUpgradeLocalData; }

        public void InitData() { }

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