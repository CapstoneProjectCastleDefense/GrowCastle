namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using Models.Blueprints;
    using Runtime.Enums;
    using Sirenix.Utilities;

    public class TalentLocalDataController : ILocalDataController
    {
        private readonly TalentBlueprint             talentBlueprint;
        private readonly TalentLocalData             talentLocalData;
        private readonly ResourceLocalDataController resourceLocalDataController;

        public TalentLocalDataController(TalentBlueprint talentBlueprint, TalentLocalData talentLocalData, ResourceLocalDataController resourceLocalDataController)
        {
            this.talentBlueprint             = talentBlueprint;
            this.talentLocalData             = talentLocalData;
            this.resourceLocalDataController = resourceLocalDataController;
        }

        public void InitData()
        {
            if (this.talentLocalData.TalentData.Count > 0) return;
            this.talentBlueprint.ForEach(talent =>
            {
                this.talentLocalData.TalentData.Add(talent.Key, 0);
            });
        }

        public int GetTalentLevel(TalentType talentType) { return this.talentLocalData.TalentData[talentType]; }

        public Dictionary<TalentType, int> GetAllTalentLocalData => this.talentLocalData.TalentData;

        public bool CheckTalentIsMaxLevel(TalentType talentType)
        {
            var talentLevel = this.talentLocalData.TalentData[talentType];

            return talentLevel >= this.talentBlueprint.GetDataById(talentType).TalentLevelToDataRecords.Count - 1;
        }

        public float GetTalentEffect(TalentType talentType)
        {
            return this.talentBlueprint[talentType]
                    .TalentLevelToDataRecords[this.GetTalentLevel(TalentType.IncreaseArcherAttackSpeed)].EffectValue
                / 100;
        }

        public bool LevelUpTalent(TalentType talentType)
        {
            if (this.CheckTalentIsMaxLevel(talentType)) return false;

            var talentLevel     = this.talentLocalData.TalentData[talentType] + 1;
            var talentPointNeed = this.talentBlueprint.GetDataById(talentType).TalentLevelToDataRecords[talentLevel].TalentPointNeed;

            if (!this.resourceLocalDataController.SpendResource(ResourceType.TalentPoint, talentPointNeed)) return false;
            this.talentLocalData.TalentData[talentType]++;

            return true;
        }
    }
}