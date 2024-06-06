namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using System.Linq;
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
        }

        public CastleRecord GetCurrentCastle() => this.castleBlueprint.GetDataById(this.castleLocalData.Level);

        public List<CastleLocalData.SlotData> GetAllSlotData() => this.castleLocalData.ListSlotData;

        public List<CastleLocalData.BlockData> GetAllBlockData() => this.castleLocalData.ListBlockData;

        public List<CastleLocalData.SlotData> GetAllUnlockedSlotData()
        {
            return this.castleLocalData.ListSlotData.Where(e => e.IsUnlock).ToList();
        }

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