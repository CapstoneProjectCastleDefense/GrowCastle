namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;

    public class ArcherLocalData : ILocalDataHaveController<ArcherLocalDataController>
    {
        public int              currentUpgradeIndex;
        public List<ArcherData> listArcher;
        public void Init()
        {
            this.currentUpgradeIndex = 0;
            this.listArcher          = new List<ArcherData>();
            for (int i = 0; i < 6; i++)
            {
                this.listArcher.Add(new(){index = i, level = 1, isUnlock = false});
            }
            this.listArcher[0].isUnlock = true;
            this.listArcher[1].isUnlock = true;
            this.listArcher[2].isUnlock = true;
        }
    }

    public class ArcherData
    {
        public int  index;
        public int  level;
        public bool isUnlock;
    }
}