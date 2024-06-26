﻿namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using System.Diagnostics;
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
        }

        public List<SlotData> GetAllSlotData => this.slotLocalData.SlotData;

        public void UnlockSlot(List<string> slotId)
        {
            if (slotId.Contains("10")) UnlockFirstTower();
            foreach (var slot in slotId)
            {
                SlotData slotData = this.slotLocalData.SlotData.First(e => e.SlotId.ToString().Equals(slot));
                slotData.IsUnlock = true;
            }
        }

        private void UnlockFirstTower() {
            this.slotLocalData.SlotData.First(slot => slot.SlotId.Equals(10)).DeployObjectId = "Xel'Naga";
        }

        public SlotRecord GetSlotDataRecord(int slotId) => this.slotBlueprint.GetDataById(slotId);

        public SlotData GetSlotData(int slotId) => this.slotLocalData.SlotData.First(e => e.SlotId == slotId);
        public void InitData()
        {
            if (this.slotLocalData.SlotData.Count == 0)
            {
                this.slotBlueprint.ForEach(slot => { 
                    this.slotLocalData.SlotData.Add(new() { SlotId = slot.Key, SlotType = slot.Value.SlotType, IsUnlock = false });
                });
                this.slotLocalData.SlotData[0].IsUnlock       = true;
                this.slotLocalData.SlotData[0].DeployObjectId = "Wizard";
            }
        }
    }
}