namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using System.Linq;

    public class ArcherLocalDataController : ILocalDataController
    {
        private readonly ArcherLocalData archerLocalData;

        public ArcherLocalDataController(ArcherLocalData archerLocalData)
        {
            this.archerLocalData = archerLocalData;
            if (this.archerLocalData.listArcher == null)
            {
                this.archerLocalData.Init();
            }
        }

        public List<ArcherData> GetAllArcher() => this.archerLocalData.listArcher;

        public List<ArcherData> GetAllUnlockedArcher() => this.archerLocalData.listArcher.Where(e => e.isUnlock).ToList();

        public void UnlockArcher()
        {
            this.archerLocalData.listArcher[this.archerLocalData.currentUpgradeIndex].isUnlock = true;
            this.archerLocalData.currentUpgradeIndex++;
            if (this.archerLocalData.currentUpgradeIndex >= 6)
            {
                this.archerLocalData.currentUpgradeIndex = 0;
            }
        }
    }
}