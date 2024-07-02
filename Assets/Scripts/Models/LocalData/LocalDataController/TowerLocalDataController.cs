namespace Models.LocalData.LocalDataController
{
    using Models.Blueprints;
    using Sirenix.Utilities;
    using UnityEngine;

    public class TowerLocalDataController : ILocalDataController {
        private readonly TowerLocalData towerLocalData;
        private readonly TowerBlueprint towerBlueprint;

        public TowerLocalDataController(TowerLocalData towerLocalData, TowerBlueprint towerBlueprint) {
            this.towerLocalData = towerLocalData;
            this.towerBlueprint = towerBlueprint;
        }

        public TowerRecord GetTowerRecord(string towerId) { return this.towerBlueprint.GetDataById(towerId); }

        public void InitData() {
            if (this.towerLocalData.listTowerData.Count == 0)
            {
                this.towerLocalData.listTowerData = new();
                this.towerBlueprint.ForEach(tower =>
                {
                    this.towerLocalData.listTowerData.Add(new()
                    {
                        id = tower.Key,
                        towerHeroStatus = HeroStatus.Lock,
                        level = 1
                    });
                    
                });
                this.towerLocalData.listTowerData[0].towerHeroStatus = HeroStatus.UnLock;
                
            }
        }
    }
}