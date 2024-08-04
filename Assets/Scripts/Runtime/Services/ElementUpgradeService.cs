namespace Runtime.Services
{
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;

    public class ElementUpgradeService
    {
        private readonly ElementUpgradeLocalDataController elementUpgradeLocalDataController;
        private readonly ElementUpgradeBlueprint           elementUpgradeBlueprint;
        public ElementUpgradeService(ElementUpgradeLocalDataController elementUpgradeLocalDataController,
            ElementUpgradeBlueprint elementUpgradeBlueprint)
        {
            this.elementUpgradeLocalDataController = elementUpgradeLocalDataController;
            this.elementUpgradeBlueprint           = elementUpgradeBlueprint;
        }

        public int GetElementLevel(string elementId)
        {
            var elementUpgradeData = this.elementUpgradeLocalDataController.GetElementUpgradeData(elementId);
            return elementUpgradeData.Level;
        }

        public float GetUpgradeCost(string elementId)
        {
            var elementUpgradeData = this.elementUpgradeLocalDataController.GetElementUpgradeData(elementId);
            var level              = elementUpgradeData.Level;

            var elementUpgradeRecord      = this.elementUpgradeBlueprint.GetDataById(elementId);
            var baseCost                  = elementUpgradeRecord.BaseCost;
            var costChangePerInterval     = elementUpgradeRecord.CostChangePerInterval;
            var levelIntervalToChangeCost = elementUpgradeRecord.LevelIntervalToChangeCost;

            var totalInterval = level / levelIntervalToChangeCost;
            var apSeriesTotal = this.GetSumOfAPSeriesFloat(totalInterval, baseCost, costChangePerInterval);
            var leftOverLevel = level - (totalInterval * levelIntervalToChangeCost);
            var currentCost   = this.GetNthValueInApSeriesFloat(totalInterval, baseCost, costChangePerInterval);

            return apSeriesTotal * levelIntervalToChangeCost + leftOverLevel * currentCost;
        }

        public float GetCurrentAttack(string elementId)
        {
            var elementUpgradeData = this.elementUpgradeLocalDataController.GetElementUpgradeData(elementId);
            var level              = elementUpgradeData.Level;

            var elementUpgradeRecord      = this.elementUpgradeBlueprint.GetDataById(elementId);
            var baseAttack                = elementUpgradeRecord.BaseAttack;
            var attackEnhancePerInterval  = elementUpgradeRecord.AttackEnhancePerInterval;
            var levelIntervalToChange = elementUpgradeRecord.LevelIntervalToChangeCost;

            var totalInterval = level / levelIntervalToChange;
            var apSeriesTotal = this.GetSumOfAPSeriesFloat(totalInterval, baseAttack, attackEnhancePerInterval);
            var leftOverLevel = level - (totalInterval * levelIntervalToChange);
            var currentEnhancementValue   = this.GetNthValueInApSeriesFloat(totalInterval, baseAttack, attackEnhancePerInterval);

            return apSeriesTotal * levelIntervalToChange + leftOverLevel * currentEnhancementValue;
        }

        private float GetSumOfAPSeriesFloat(int series, float firstTerm, float dif)   { return (float)series * (2 * firstTerm + (series - 1) * dif) / 2; }
        private float GetNthValueInApSeriesFloat(int nth, float firstTerm, float dif) { return firstTerm + (nth - 1) * dif; }

        private int GetSumOfAPSeriesInt(int series, int firstTerm, int interval) { return series * (2 * firstTerm + (series - 1) * interval) / 2; }
    }
}