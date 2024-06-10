﻿namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Castles;

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

        public CastleStat GetCasteStat()
        {
            var result     = new CastleStat();
            var configData = this.castleConfigBlueprint.First().Value;
            result.Hp            = configData.BaseHP;
            result.Mp            = configData.BaseMP;
            result.GoldToUpgrade = configData.BaseGoldNeedToUpgrade;

            return result;
        }
    }
}