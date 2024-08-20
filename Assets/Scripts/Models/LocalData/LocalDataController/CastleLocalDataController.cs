namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Blueprints;
    using R3;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Signals.Quests;
    using Runtime.StaticValues;
    using Zenject;

    public class CastleLocalDataController : ILocalDataController
    {
        private          CastleLocalData             castleLocalData;
        private readonly CastleConfigBlueprint       castleConfigBlueprint;
        private readonly CastleBlueprint             castleBlueprint;
        private readonly BlockBlueprint              blockBlueprint;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly TalentLocalDataController   talentLocalDataController;
        private readonly TalentBlueprint             talentBlueprint;
        private readonly SignalBus                   signalBus;
        private readonly SlotLocalDataController     slotLocalDataController;

        public CastleLocalDataController(
            CastleLocalData             castleLocalData,
            CastleConfigBlueprint       castleConfigBlueprint,
            CastleBlueprint             castleBlueprint,
            SlotLocalDataController     slotLocalDataController,
            BlockBlueprint              blockBlueprint,
            ResourceLocalDataController resourceLocalDataController,
            TalentLocalDataController   talentLocalDataController,
            TalentBlueprint             talentBlueprint,
            SignalBus                   signalBus
        )
        {
            this.castleLocalData             = castleLocalData;
            this.castleConfigBlueprint       = castleConfigBlueprint;
            this.castleBlueprint             = castleBlueprint;
            this.slotLocalDataController     = slotLocalDataController;
            this.blockBlueprint              = blockBlueprint;
            this.resourceLocalDataController = resourceLocalDataController;
            this.talentLocalDataController   = talentLocalDataController;
            this.talentBlueprint             = talentBlueprint;
            this.signalBus                   = signalBus;
        }

        #region Castle

        public CastleRecord GetCurrentCastle() => this.castleBlueprint.GetDataById(this.castleLocalData.Level);

        public bool UpgradeCastle()
        {
            if (!this.resourceLocalDataController.SpendResource(ResourceType.Gold, this.GetGoldToUpgrade())) return false;
            this.castleLocalData.Level++;
            if (!this.castleBlueprint.ContainsKey(this.castleLocalData.Level))
            {
                return true;
            }
            if (this.castleLocalData.Level <= this.castleBlueprint.Count)
            {
                var newBlockUnlockId    = this.castleBlueprint.GetDataById(this.castleLocalData.Level).BlockUnlock;
                var newBlockUnlockLevel = this.castleBlueprint.GetDataById(this.castleLocalData.Level).BlockUnlockLevel;
                this.UnlockNewBlock(newBlockUnlockId, newBlockUnlockLevel);
                this.UnlockNewSlot(this.castleBlueprint.GetDataById(this.castleLocalData.Level).SlotUnlock);
            }

            this.signalBus.Fire(new QuestTriggerSignal() { TriggerSignalId = QuestTriggerSignalId.UpgradeCastle, Value = 1 });

            return true;
        }

        public float GetGoldToUpgrade() { return this.castleConfigBlueprint.BaseGoldNeedToUpgrade + this.castleConfigBlueprint.BaseGoldNeedToUpgrade * this.castleLocalData.Level * this.castleConfigBlueprint.CoefficientGold; }

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

        public Dictionary<StatEnum, (Type, Object)> GetCastleStat()
        {
            var result     = new Dictionary<StatEnum, (Type, Object)>();
            var configData = this.castleConfigBlueprint;
            var health = configData.BaseHP
                + this.castleLocalData.Level * configData.BaseHP * 0.3f;

            result.Add(StatEnum.MaxHealth, (configData.BaseHP.GetType(), health));
            result.Add(StatEnum.Health, (configData.BaseHP.GetType(), health)); //TODO: *10000 for testing, change to local data later
            result.Add(StatEnum.Mana, (configData.BaseMP.GetType(), configData.BaseMP + 0.2f * this.castleLocalData.Level * configData.BaseMP));
            result.Add(StatEnum.MaxMana, (configData.BaseMP.GetType(), configData.BaseMP + 0.2f * this.castleLocalData.Level * configData.BaseMP));
            this.UpdateStats(result.GetStat<float>(StatEnum.MaxHealth), result.GetStat<float>(StatEnum.MaxMana));

            return result;
        }

        public void UpdateStats(float health, float mana)
        {
            this.castleLocalData.Stats[StatEnum.Health].Value = health;
            this.castleLocalData.Stats[StatEnum.Mana].Value   = mana;
        }

        public ReactiveProperty<float> GetStats(StatEnum statType) => this.castleLocalData.Stats[statType];

        public void InitData()
        {
            if (this.castleLocalData.ListBlockData.Count > 0) return;
            this.castleLocalData.Level         = 1;
            this.castleLocalData.ListBlockData = new();
            this.blockBlueprint.ForEach(blockData =>
            {
                this.castleLocalData.ListBlockData.Add(new() { BlockId = blockData.Value.Id, BlockLevel = 1, IsUnlock = false });
            });
            this.castleLocalData.ListBlockData.First().IsUnlock = true;
            this.castleLocalData.Stats.Add(StatEnum.Health, new ReactiveProperty<float>(500f));
            this.castleLocalData.Stats.Add(StatEnum.Mana, new ReactiveProperty<float>(100f));
        }

        internal int GetCurrentUpgradeLevel()
        {
            return this.castleLocalData.Level;
        }
    }
}