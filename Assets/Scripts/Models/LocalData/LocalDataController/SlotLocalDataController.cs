namespace Models.LocalData.LocalDataController {
    using Models.Blueprints;
    using System.Collections.Generic;

    public class SlotLocalDataController : ILocalDataController {
        private SlotLocalData slotLocalData;
        private SlotBlueprint slotBlueprint;

        public SlotLocalDataController(SlotLocalData slotLocalData, SlotBlueprint slotBlueprint) {
            this.slotLocalData = slotLocalData;
            this.slotBlueprint = slotBlueprint;
        }

        public SlotRecord GetCurrentSlot() => this.slotBlueprint.GetDataById(this.slotLocalData.Id);

        public string GetSlotType() => this.slotLocalData.SlotType;

        public bool IsSlotUnlocked() => this.slotLocalData.IsUnlock;

        public bool IsSlotOccupied() {
            return IsSlotUnlocked()? this.slotLocalData.IsOccupied : false;
        }

        public List<SlotLocalData.HeroData> GetAllHeroData() => this.slotLocalData.HeroDataList;
        public List<SlotLocalData.TowerData> GetAllTowerData() => this.slotLocalData.TowerDataList;

    }
}