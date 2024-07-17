namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Blueprints;
    using Runtime.Signals.Quests;
    using Runtime.StaticValues;
    using Zenject;

    public class ArcherLocalDataController : ILocalDataController
    {
        private readonly ArcherLocalData             archerLocalData;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly ArcherConfigBlueprint       archerConfigBlueprint;
        private readonly SignalBus                   signalBus;

        public ArcherLocalDataController(ArcherLocalData archerLocalData, ResourceLocalDataController resourceLocalDataController, ArcherConfigBlueprint archerConfigBlueprint, SignalBus signalBus)
        {
            this.archerLocalData             = archerLocalData;
            this.resourceLocalDataController = resourceLocalDataController;
            this.archerConfigBlueprint       = archerConfigBlueprint;
            this.signalBus                   = signalBus;
        }

        public List<ArcherData> GetAllArcher() => this.archerLocalData.ListArcher;

        public List<ArcherData> GetAllUnlockedArcher() => this.archerLocalData.ListArcher.Where(e => e.isUnlock).ToList();

        public ArcherData UnlockArcher()
        {
            if (!this.resourceLocalDataController.SpendResource(ResourceType.Gold, this.GetGoldNeedToUpgrade())) return null;
            this.archerLocalData.CurrentLevel++;
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
            this.signalBus.Fire(new QuestTriggerSignal(){TriggerSignalId = QuestTriggerSignalId.UpgradeArcher});

            return this.archerLocalData.ListArcher[this.archerLocalData.CurrentUpgradeIndex];
        }

        private int GetCurrentMaxLevelOfArcher() { return this.archerLocalData.ListArcher.Max(e => e.level); }

        public float GetGoldNeedToUpgrade() { return this.archerConfigBlueprint.BaseGold * this.GetCurrentMaxLevelOfArcher() * this.archerConfigBlueprint.Coefficient; }

        public int GetCurrentUpgradeLevel() { return this.archerLocalData.CurrentLevel; }
        public void InitData()
        {
            if (this.archerLocalData.ListArcher.Count == 0)
            {
                this.archerLocalData.Init();
            }
        }
    }
}