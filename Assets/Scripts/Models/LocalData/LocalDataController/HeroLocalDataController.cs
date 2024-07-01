namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Blueprints;
    using Sirenix.Utilities;

    public class HeroLocalDataController : ILocalDataController
    {
        private readonly HeroLocalData               heroLocalData;
        private readonly HeroBlueprint               heroBlueprint;
        private readonly HeroConfigBlueprint         heroConfigBlueprint;
        private readonly ResourceLocalDataController resourceLocalDataController;

        public HeroLocalDataController(HeroLocalData heroLocalData, HeroBlueprint heroBlueprint, HeroConfigBlueprint heroConfigBlueprint, ResourceLocalDataController resourceLocalDataController)
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
                this.heroBlueprint.ForEach(hero =>
                {
                    this.heroLocalData.listHeroData.Add(new() { id = hero.Key, heroStatus = HeroStatus.Lock, level = 1 });
                });
                this.heroLocalData.listHeroData[0].heroStatus = HeroStatus.UnLock;
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
                heroStatus    = this.heroLocalData.listHeroData.First(e => e.id.Equals(heroId)).heroStatus,
            };

            return heroRuntimeData;
        }

        public List<HeroRuntimeData> GetAllHeroData()
        {
            return this.heroLocalData.listHeroData.Select(data => this.GetHeroRuntimeData(data.id)).ToList();
        }

        public void EquipHero(string heroId)
        {
            var heroLocalData = this.GetHeroLocalData(heroId);
            if (heroLocalData.heroStatus == HeroStatus.UnLock)
            {
                heroLocalData.heroStatus = HeroStatus.Equip;
            }
        }

        public bool UnLockHero(string heroId)
        {
            var heroData = this.GetHeroRuntimeData(heroId);

            if (!heroData.heroStatus.Equals(HeroStatus.Lock)) return false;
            if (!this.resourceLocalDataController.SpendResource(ResourceType.Gold, heroData.resourceValue)) return false;
            this.GetHeroLocalData(heroId).heroStatus = HeroStatus.UnLock;

            return true;
        }

        public bool UpgradeHero(string heroId)
        {
            var heroData     = this.GetHeroRuntimeData(heroId);

            if (heroData.heroStatus.Equals(HeroStatus.Lock)) return false;

            if (!this.resourceLocalDataController.SpendResource(ResourceType.Gold, heroData.resourceValue)) return false;
            this.GetHeroLocalData(heroId).level++;

            return true;
        }
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