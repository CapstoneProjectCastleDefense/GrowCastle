namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Blueprints;
    using Runtime.Enums;
    using Sirenix.Utilities;

    public class HeroLocalDataController : ILocalDataController
    {
        private readonly HeroLocalData               heroLocalData;
        private readonly HeroBlueprint               heroBlueprint;
        private readonly HeroConfigBlueprint         heroConfigBlueprint;
        private readonly ResourceLocalDataController resourceLocalDataController;

        public HeroLocalDataController(HeroLocalData heroLocalData, HeroBlueprint heroBlueprint, HeroConfigBlueprint heroConfigBlueprint,
            ResourceLocalDataController resourceLocalDataController)
        {
            this.heroLocalData               = heroLocalData;
            this.heroBlueprint               = heroBlueprint;
            this.heroConfigBlueprint         = heroConfigBlueprint;
            this.resourceLocalDataController = resourceLocalDataController;
        }

        public void InitData()
        {
            if (this.heroLocalData.listHeroData.Count == 0)
            {
                this.heroLocalData.listHeroData = new();
                this.heroBlueprint.ForEach(hero => { this.heroLocalData.listHeroData.Add(new() { id = hero.Key, level = 1, listEquipmentId = new() }); });
                this.heroLocalData.listHeroData[0].HeroStatus.Value = HeroStatus.Equip;
            }
        }

        public HeroData GetHeroLocalData(string heroId) => this.heroLocalData.listHeroData.First(e => e.id.Equals(heroId));

        public HeroRuntimeData GetHeroRuntimeData(string heroId)
        {
            var heroLocalData    = this.GetHeroLocalData(heroId);
            var heroConfigRecord = this.heroConfigBlueprint.GetDataById(heroId);
            var heroRuntimeData = new HeroRuntimeData()
            {
                heroRecord    = this.heroBlueprint.GetDataById(heroId),
                attack        = heroConfigRecord.BaseAttack,
                attackSpeed   = heroConfigRecord.BaseAttackSpeed,
                avatar        = heroConfigRecord.LevelToConfigRecords[heroLocalData.level].Avatar,
                resourceValue = heroConfigRecord.BaseResource,
                resourceType  = heroConfigRecord.ResourceType,
                heroStatus    = this.heroLocalData.listHeroData.First(e => e.id.Equals(heroId)).HeroStatus.Value,
            };

            return heroRuntimeData;
        }

        public List<HeroRuntimeData> GetAllHeroRuntimeData() { return this.heroLocalData.listHeroData.Select(data => this.GetHeroRuntimeData(data.id)).ToList(); }

        public void EquipHero(string heroId)
        {
            var heroLocalData = this.GetHeroLocalData(heroId);
            if (heroLocalData.HeroStatus.Value == HeroStatus.UnLock)
            {
                heroLocalData.HeroStatus.Value = HeroStatus.Equip;
            }
        }

        public void UnEquipHero(string heroId)
        {
            var heroLocalData = this.GetHeroLocalData(heroId);
            if (heroLocalData.HeroStatus.Value == HeroStatus.Equip)
            {
                heroLocalData.HeroStatus.Value = HeroStatus.UnLock;
            }
        }

        public bool UnLockHero(string heroId)
        {
            var heroData = this.GetHeroRuntimeData(heroId);

            if (!heroData.heroStatus.Equals(HeroStatus.Lock)) return false;
            if (!this.resourceLocalDataController.SpendResource(ResourceType.Gold, heroData.resourceValue)) return false;
            this.GetHeroLocalData(heroId).HeroStatus.Value = HeroStatus.UnLock;

            return true;
        }

        public bool UpgradeHero(string heroId)
        {
            var heroData = this.GetHeroRuntimeData(heroId);

            if (heroData.heroStatus.Equals(HeroStatus.Lock)) return false;

            if (!this.resourceLocalDataController.SpendResource(ResourceType.Gold, heroData.resourceValue)) return false;
            this.GetHeroLocalData(heroId).level++;

            return true;
        }

        public List<string> GetEquipments(string heroId) { return this.GetHeroLocalData(heroId).listEquipmentId; }

        public void EquipEquipment(string heroId, string equipmentId) { this.GetHeroLocalData(heroId).listEquipmentId.Add(equipmentId); }

        public void UnEquipEquipment(string heroId, string equipmentId) { this.GetHeroLocalData(heroId).listEquipmentId.Remove(equipmentId); }
    }

    public class HeroRuntimeData
    {
        public HeroRecord   heroRecord;
        public float        attack;
        public float        attackSpeed;
        public float        resourceValue;
        public ResourceType resourceType;
        public string       avatar;
        public HeroStatus   heroStatus;
    }
}