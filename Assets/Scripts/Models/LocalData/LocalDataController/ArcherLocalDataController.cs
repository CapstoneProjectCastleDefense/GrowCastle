namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Blueprints;

    public class ArcherLocalDataController : ILocalDataController
    {
        private readonly ArcherLocalData             archerLocalData;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly ArcherConfigBlueprint       archerConfigBlueprint;

        public ArcherLocalDataController(ArcherLocalData archerLocalData, ResourceLocalDataController resourceLocalDataController, ArcherConfigBlueprint archerConfigBlueprint)
        {
            this.archerLocalData             = archerLocalData;
            this.resourceLocalDataController = resourceLocalDataController;
            this.archerConfigBlueprint       = archerConfigBlueprint;
        }

        public List<ArcherData> GetAllArcher() => this.archerLocalData.ListArcher;

        public List<ArcherData> GetAllUnlockedArcher() => this.archerLocalData.ListArcher.Where(e => e.isUnlock).ToList();

        public ArcherData UnlockArcher()
        {
            if (!this.resourceLocalDataController.SpendResource(ResourceType.Gold, this.archerConfigBlueprint.BaseGold)) return null;

            this.archerLocalData.CurrentUpgradeIndex++;
            if (this.archerLocalData.CurrentUpgradeIndex >= 6)
            {
                this.archerLocalData.CurrentUpgradeIndex = 0;
            }

            var currentArcher = this.archerLocalData.ListArcher[this.archerLocalData.CurrentUpgradeIndex];
            if (currentArcher.isUnlock)
            {
                currentArcher.level++;
            }
            else
            {
                this.archerLocalData.ListArcher[this.archerLocalData.CurrentUpgradeIndex].isUnlock = true;
            }

            return this.archerLocalData.ListArcher[this.archerLocalData.CurrentUpgradeIndex];
        }

        public void InitData()
        {
            if (this.archerLocalData.ListArcher.Count == 0)
            {
                this.archerLocalData.Init();
            }
        }
    }
}