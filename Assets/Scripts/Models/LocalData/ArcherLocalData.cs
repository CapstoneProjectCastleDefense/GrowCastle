namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;

    public class ArcherLocalData : ILocalDataHaveController<ArcherLocalDataController>
    {
        public int              CurrentUpgradeIndex;
        public List<ArcherData> ListArcher = new();
        public void Init()
        {
            this.CurrentUpgradeIndex = 0;
            this.ListArcher          = new List<ArcherData>();
            for (int i = 0; i < 6; i++)
            {
                this.ListArcher.Add(new(){index = i, level = 1, isUnlock = false});
            }
            this.ListArcher[0].isUnlock = true;
        }
    }

    public class ArcherData
    {
        public int  index;
        public int  level;
        public bool isUnlock;
    }
}