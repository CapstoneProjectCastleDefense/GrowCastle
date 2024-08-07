namespace Runtime.Services
{
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;

    public class HeroUpgradeService
    {
        private readonly ElementUpgradeBlueprint elementUpgradeBlueprint;
        private readonly HeroLocalDataController heroLocalDataController;
        public HeroUpgradeService(
            ElementUpgradeBlueprint elementUpgradeBlueprint,
            HeroLocalDataController heroLocalDataController)
        {
            this.elementUpgradeBlueprint = elementUpgradeBlueprint;
            this.heroLocalDataController = heroLocalDataController;
        }

        public int GetHeroLevel(string heroId)
        {
            var elementUpgradeData = this.heroLocalDataController.GetHeroLocalData(heroId);
            return elementUpgradeData.Level;
        }

        public float GetUpgradeCost(string heroId)
        {
            var heroData = this.heroLocalDataController.GetHeroLocalData(heroId);
            var level    = heroData.Level;

            var elementUpgradeRecord      = this.elementUpgradeBlueprint.GetDataById(heroId);
            var baseCost                  = elementUpgradeRecord.BaseCost;
            var costChangePerInterval     = elementUpgradeRecord.CostChangePerInterval;
            var levelIntervalToChangeCost = elementUpgradeRecord.LevelIntervalToChangeCost;

            var totalInterval = level / levelIntervalToChangeCost;
            var apSeriesTotal = this.GetSumOfAPSeriesFloat(totalInterval, baseCost, costChangePerInterval);
            var leftOverLevel = level - (totalInterval * levelIntervalToChangeCost);
            var currentCost   = this.GetNthValueInApSeriesFloat(totalInterval, baseCost, costChangePerInterval);

            return apSeriesTotal * levelIntervalToChangeCost + leftOverLevel * currentCost;
        }

        public float GetCurrentAttack(string heroId)
        {
            var heroData = this.heroLocalDataController.GetHeroLocalData(heroId);
            var level    = heroData.Level;

            var elementUpgradeRecord     = this.elementUpgradeBlueprint.GetDataById(heroId);
            var baseAttack               = elementUpgradeRecord.BaseAttack;
            var attackEnhancePerInterval = elementUpgradeRecord.AttackEnhancePerInterval;
            var levelIntervalToChange    = elementUpgradeRecord.LevelIntervalToChangeCost;

            var totalInterval           = level / levelIntervalToChange;
            var apSeriesTotal           = this.GetSumOfAPSeriesFloat(totalInterval, baseAttack, attackEnhancePerInterval);
            var leftOverLevel           = level - (totalInterval * levelIntervalToChange);
            var currentEnhancementValue = this.GetNthValueInApSeriesFloat(totalInterval, baseAttack, attackEnhancePerInterval);

            return apSeriesTotal * levelIntervalToChange + leftOverLevel * currentEnhancementValue;
        }

        private float GetSumOfAPSeriesFloat(int series, float firstTerm, float dif)   { return (float)series * (2 * firstTerm + (series - 1) * dif) / 2; }
        private float GetNthValueInApSeriesFloat(int nth, float firstTerm, float dif) { return firstTerm + (nth - 1) * dif; }

        private int GetSumOfAPSeriesInt(int series, int firstTerm, int interval) { return series * (2 * firstTerm + (series - 1) * interval) / 2; }
    }
}