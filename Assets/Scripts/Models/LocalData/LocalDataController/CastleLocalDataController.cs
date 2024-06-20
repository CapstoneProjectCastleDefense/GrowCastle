namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Castles;
    using Runtime.Enums;

    public class CastleLocalDataController : ILocalDataController
    {
        private readonly CastleLocalData       castleLocalData;
        private readonly CastleConfigBlueprint castleConfigBlueprint;
        private readonly CastleBlueprint       castleBlueprint;

        public CastleLocalDataController(CastleLocalData castleLocalData, CastleConfigBlueprint castleConfigBlueprint, CastleBlueprint castleBlueprint)
        {
            this.castleLocalData       = castleLocalData;
            this.castleConfigBlueprint = castleConfigBlueprint;
            this.castleBlueprint       = castleBlueprint;
            this.InitCastleLocalData();
        }

        private void InitCastleLocalData()
        {
            if (this.castleLocalData.ListBlockData != null) return;
            this.castleLocalData.Level         = 1;
            this.castleLocalData.ListBlockData = new();
            this.castleBlueprint.ForEach(castleData =>
            {
                this.castleLocalData.ListBlockData.Add(new() { BlockId = castleData.Value.BlockUnlockId, BlockLevel = 1, IsUnlock = false });
            });
            this.castleLocalData.ListBlockData.First().IsUnlock = true;
        }

        #region Castle

        public CastleRecord GetCurrentCastle() => this.castleBlueprint.GetDataById(this.castleLocalData.Level);

        public void UpgradeCastle()
        {
            this.castleLocalData.Level++;
            var newBlockUnlockId    = this.castleBlueprint.GetDataById(this.castleLocalData.Level).BlockUnlockId;
            var newBlockUnlockLevel = this.castleBlueprint.GetDataById(this.castleLocalData.Level).BlockUnlockLevel;
            this.UnlockNewBlock(newBlockUnlockId, newBlockUnlockLevel);
            this.UnlockNewSlot(this.castleBlueprint.GetDataById(this.castleLocalData.Level).SlotUnlock);
        }

        #endregion

        #region Slot

        
        private void UnlockNewSlot(List<string> slotId)
        {
        }

        #endregion

        #region Block

        public List<CastleLocalData.BlockData> GetAllBlockData() => this.castleLocalData.ListBlockData;

        public CastleLocalData.BlockData GetBlockDataById(string blockId) => this.castleLocalData.ListBlockData.First(e => e.BlockId.Equals(blockId));

        private void UnlockNewBlock(string blockId, int blockLevel)
        {
            var blockData = this.GetBlockDataById(blockId);
            blockData.IsUnlock   = true;
            blockData.BlockLevel = blockLevel;
        }

        #endregion

        public Dictionary<StatEnum, (Type, Object)> GetCastleStat()
        {
            var result     = new Dictionary<StatEnum, (Type, Object)>();
            var configData = this.castleConfigBlueprint.First().Value;
            result.Add(StatEnum.Health, (configData.BaseHP.GetType(), configData.BaseHP*10000)); //TODO: *10000 for testing, change to local data later
            result.Add(StatEnum.Mana, (configData.BaseMP.GetType(), configData.BaseMP));

            return result;
        }
    }
}