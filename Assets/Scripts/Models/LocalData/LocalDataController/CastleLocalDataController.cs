namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Castles;
    using Runtime.Enums;
    using UnityEngine;

    public class CastleLocalDataController : ILocalDataController
    {
        private          CastleLocalData         castleLocalData;
        private readonly CastleConfigBlueprint   castleConfigBlueprint;
        private readonly CastleBlueprint         castleBlueprint;
        private readonly BlockBlueprint          blockBlueprint;
        private readonly SlotLocalDataController slotLocalDataController;

        public CastleLocalDataController(
            CastleLocalData castleLocalData,
            CastleConfigBlueprint castleConfigBlueprint,
            CastleBlueprint castleBlueprint,
            SlotLocalDataController slotLocalDataController,
            BlockBlueprint blockBlueprint)
        {
            this.castleLocalData         = castleLocalData;
            this.castleConfigBlueprint   = castleConfigBlueprint;
            this.castleBlueprint         = castleBlueprint;
            this.slotLocalDataController = slotLocalDataController;
            this.blockBlueprint          = blockBlueprint;
        }

        #region Castle

        public CastleRecord GetCurrentCastle() => this.castleBlueprint.GetDataById(this.castleLocalData.Level);

        public void UpgradeCastle()
        {
            if (this.castleBlueprint.Last().Key == (this.castleLocalData.Level))
            {
                return;
            }

            this.castleLocalData.Level++;
            var newBlockUnlockId    = this.castleBlueprint.GetDataById(this.castleLocalData.Level).BlockUnlock;
            var newBlockUnlockLevel = this.castleBlueprint.GetDataById(this.castleLocalData.Level).BlockUnlockLevel;
            this.UnlockNewBlock(newBlockUnlockId, newBlockUnlockLevel);
            this.UnlockNewSlot(this.castleBlueprint.GetDataById(this.castleLocalData.Level).SlotUnlock);
        }

        #endregion

        #region Slot

        private void UnlockNewSlot(List<string> slotUnlock)
        {
            if (slotUnlock.Count == 0)
            {
                return;
            }

            this.slotLocalDataController.UnlockSlot(slotUnlock);
        }

        #endregion

        #region Block

        public List<CastleLocalData.BlockData> GetAllBlockData() => this.castleLocalData.ListBlockData;

        public CastleLocalData.BlockData GetBlockDataById(string blockId) => this.castleLocalData.ListBlockData.FirstOrDefault(e => e.BlockId.Equals(blockId));

        private void UnlockNewBlock(List<string> blockList, int blockLevel)
        {
            foreach (var blockId in blockList)
            {
                var blockData = this.GetBlockDataById(blockId);
                blockData.IsUnlock   = true;
                blockData.BlockLevel = blockLevel;
            }
        }

        #endregion

        public Dictionary<StatEnum, (Type, System.Object)> GetCastleStat()
        {
            var result     = new Dictionary<StatEnum, (Type, System.Object)>();
            var configData = this.castleConfigBlueprint.First().Value;

            result.Add(StatEnum.MaxHealth, (configData.BaseHP.GetType(), configData.BaseHP * 1));
            result.Add(StatEnum.Health, (configData.BaseHP.GetType(), configData.BaseHP * 1)); //TODO: *10000 for testing, change to local data later
            result.Add(StatEnum.Mana, (configData.BaseMP.GetType(), configData.BaseMP));

            return result;
        }
        public void InitData()
        {
            if (this.castleLocalData.ListBlockData.Count > 0) return;
            this.castleLocalData.Level         = 1;
            this.castleLocalData.ListBlockData = new();
            this.blockBlueprint.ForEach(blockData => { this.castleLocalData.ListBlockData.Add(new() { BlockId = blockData.Value.Id, BlockLevel = 1, IsUnlock = false }); });
            this.castleLocalData.ListBlockData.First().IsUnlock = true;
        }
    }
}