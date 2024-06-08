namespace Models.LocalData {
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;

    public class SlotLocalData : ILocalDataHaveController<SlotLocalDataController> {
        public string Id;
        public string SlotType;
        public bool IsUnlock;
        public bool IsOccupied;
        public List<HeroData> HeroDataList;
        public List<TowerData> TowerDataList;

        public void Init() {}

        public class HeroData {
        }

        public class TowerData {
        }
    }


}