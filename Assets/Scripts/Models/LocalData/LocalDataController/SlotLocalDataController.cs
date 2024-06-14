namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Blueprints;
    using Sirenix.Utilities;

    public class SlotLocalDataController : ILocalDataController
    {
        private readonly SlotLocalData slotLocalData;
        private readonly SlotBlueprint slotBlueprint;
        public SlotLocalDataController(SlotLocalData slotLocalData, SlotBlueprint slotBlueprint)
        {
            this.slotLocalData = slotLocalData;
            this.slotBlueprint = slotBlueprint;
            if (this.slotLocalData.SlotData == null)
            {
                this.slotLocalData.SlotData = new();
                this.slotBlueprint.ForEach(slot =>
                {
                    this.slotLocalData.SlotData.Add(new() { SlotId = slot.Key, SlotType = slot.Value.SlotType, IsUnlock = false });
                });
                this.slotLocalData.SlotData[0].IsUnlock       = true;
                this.slotLocalData.SlotData[0].DeployObjectId = "Knight";
            }
        }

        public List<SlotData> GetAllSlotData => this.slotLocalData.SlotData;

        public void UnlockSlot(int slotId) { this.slotLocalData.SlotData.First(e => e.SlotId == slotId).IsUnlock = true; }

        public SlotRecord GetSlotDataRecord(int slotId) => this.slotBlueprint.GetDataById(slotId);

        public SlotData GetSlotData(int slotId) => this.slotLocalData.SlotData.First(e => e.SlotId == slotId);
    }
}