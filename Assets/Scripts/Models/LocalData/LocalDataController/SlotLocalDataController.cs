namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;
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
            foreach (var slot in slotId)
            {
                this.slotLocalData.SlotData.First(e => e.SlotId.ToString().Equals(slot)).IsUnlock = true;
            }
        }

        public void EquipCharacter(int slotId, string characterId)
        {
            this.GetSlotData(slotId).DeployObjectId = characterId;
        }

        public void UnEquipCharacter(int slotId)
        {
            this.GetSlotData(slotId).DeployObjectId = null;
        }

        public SlotData GetSlotHoldHero(string heroId)
        {
            if (heroId.IsNullOrEmpty() || !this.slotLocalData.SlotData.Any(e => e.DeployObjectId != null && e.DeployObjectId.Equals(heroId))) return null;
            return this.slotLocalData.SlotData.First(e => e.DeployObjectId != null && e.DeployObjectId.Equals(heroId));
        }

        public SlotRecord GetSlotDataRecord(int slotId) => this.slotBlueprint.GetDataById(slotId);

        public SlotData GetSlotData(int slotId) => this.slotLocalData.SlotData.First(e => e.SlotId == slotId);

        public void InitData()
        {
            if (this.slotLocalData.SlotData.Count == 0)
            {
                this.slotBlueprint.ForEach(slot =>
                {
                    this.slotLocalData.SlotData.Add(new() { SlotId = slot.Key, SlotType = slot.Value.SlotType, IsUnlock = false });
                });
                this.slotLocalData.SlotData[0].IsUnlock       = true;
                this.slotLocalData.SlotData[0].DeployObjectId = "Knight";
            }
        }
    }
}