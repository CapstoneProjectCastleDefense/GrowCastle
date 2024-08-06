namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models.Blueprints;
    using Runtime.Enums;
    using Runtime.Signals.Quests;
    using Runtime.StaticValues;
    using Sirenix.Utilities;
    using Zenject;

    public class HeroLocalDataController : ILocalDataController
    {
        private readonly HeroLocalData               heroLocalData;
        private readonly HeroBlueprint               heroBlueprint;
        private readonly HeroConfigBlueprint         heroConfigBlueprint;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly SignalBus                   signalBus;

        public HeroLocalDataController(HeroLocalData heroLocalData, HeroBlueprint heroBlueprint, HeroConfigBlueprint heroConfigBlueprint,
            ResourceLocalDataController resourceLocalDataController, SignalBus signalBus)
        {
            this.heroLocalData               = heroLocalData;
            this.heroBlueprint               = heroBlueprint;
            this.heroConfigBlueprint         = heroConfigBlueprint;
            this.resourceLocalDataController = resourceLocalDataController;
            this.signalBus                   = signalBus;
        }

        public void InitData()
        {
            if (this.heroLocalData.IdToHeroData.Count == 0)
            {
                this.heroLocalData.IdToHeroData = new();
                this.heroBlueprint.ForEach(hero => { this.heroLocalData.IdToHeroData.Add(hero.Key, new() { Id = hero.Key, Level = 1, ListEquipmentId = new() }); });
                this.heroLocalData.IdToHeroData.First().Value.HeroStatus.Value = HeroStatus.Equip;
            }
        }

        public HeroRuntimeData GetHeroRuntimeData(string heroId)
        {
            var heroConfigRecord = this.heroConfigBlueprint.GetDataById(heroId);
            var heroRuntimeData = new HeroRuntimeData()
            {
                heroRecord    = this.heroBlueprint.GetDataById(heroId),
                attack        = heroConfigRecord.BaseAttack,
                attackSpeed   = heroConfigRecord.BaseAttackSpeed,
                avatar        = heroConfigRecord.LevelToConfigRecords[1].Avatar,
                resourceValue = heroConfigRecord.BaseResource,
                resourceType  = heroConfigRecord.ResourceType,
                heroStatus    = this.heroLocalData.IdToHeroData[heroId].HeroStatus.Value,
            };

            return heroRuntimeData;
        }

        public List<HeroRuntimeData> GetAllHeroRuntimeData() { return this.heroLocalData.IdToHeroData.Select(data => this.GetHeroRuntimeData(data.Key)).ToList(); }

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
            this.signalBus.Fire(new QuestTriggerSignal(){TriggerSignalId = QuestTriggerSignalId.UnlockHero, Value = 1});
            return true;
        }

        public List<string> GetEquipments(string heroId) { return this.GetHeroLocalData(heroId).ListEquipmentId; }

        public void EquipEquipment(string heroId, string equipmentId) { this.GetHeroLocalData(heroId).ListEquipmentId.Add(equipmentId); }

        public void UnEquipEquipment(string heroId, string equipmentId) { this.GetHeroLocalData(heroId).ListEquipmentId.Remove(equipmentId); }

        public HeroData GetHeroLocalData(string heroId)
        {
            if (!this.heroLocalData.IdToHeroData.TryGetValue(heroId, out var elementUpgradeData))
            {
                throw new Exception($"Not found element upgrade data of element: {heroId}");
            }

            return elementUpgradeData;
        }

        public void UpgradeHero(string heroId, int levelUpgradeAmount = 1)
        {
            if (!this.heroLocalData.IdToHeroData.TryGetValue(heroId, out var elementUpgradeData))
            {
                throw new Exception($"Not found element upgrade data of element: {heroId}");
            }

            elementUpgradeData.Level += levelUpgradeAmount;
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