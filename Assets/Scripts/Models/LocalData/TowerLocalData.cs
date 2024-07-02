namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;

    public class TowerLocalData : ILocalDataHaveController<TowerLocalDataController> {
        public List<TowerData> listTowerData = new();
        public void Init() {

        }
    }

    public class TowerData
    {
        public string id;
        public int level;
        public HeroStatus towerHeroStatus;
    }
}