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
            if (this.archerLocalData.ListArcher == null)
            {
                this.archerLocalData.Init();
            }
        }

        public List<ArcherData> GetAllArcher() => this.archerLocalData.ListArcher;

        public List<ArcherData> GetAllUnlockedArcher() => this.archerLocalData.ListArcher.Where(e => e.isUnlock).ToList();

        public ArcherData UnlockArcher()
        {
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
    }
}