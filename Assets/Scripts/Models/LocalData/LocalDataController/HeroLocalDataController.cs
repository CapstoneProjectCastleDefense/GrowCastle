namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Models.Blueprints;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Services;
    using Runtime.Signals.Quests;
    using Runtime.StaticValues;
    using Sirenix.Utilities;
    using UnityEngine;
    using Zenject;

    public class HeroLocalDataController : ILocalDataController
    {
        private readonly HeroLocalData                       heroLocalData;
        private readonly HeroBlueprint                       heroBlueprint;
        private readonly HeroConfigBlueprint                 heroConfigBlueprint;
        private readonly ResourceLocalDataController         resourceLocalDataController;
        private readonly SignalBus                           signalBus;
        private readonly EvolutionInfoBlueprint              evolutionInfoBlueprint;
        private readonly ElementEvolutionLocalDataController elementEvolutionLocalDataController;
        private readonly InventoryLocalDataController        inventoryLocalDataController;

        public HeroLocalDataController(
            HeroLocalData heroLocalData,
            HeroBlueprint heroBlueprint,
            HeroConfigBlueprint heroConfigBlueprint,
            ResourceLocalDataController resourceLocalDataController,
            SignalBus signalBus,
            EvolutionInfoBlueprint evolutionInfoBlueprint,
            ElementEvolutionLocalDataController elementEvolutionLocalDataController,
            InventoryLocalDataController inventoryLocalDataController
        )
        {
            this.heroLocalData                       = heroLocalData;
            this.heroBlueprint                       = heroBlueprint;
            this.heroConfigBlueprint                 = heroConfigBlueprint;
            this.resourceLocalDataController         = resourceLocalDataController;
            this.signalBus                           = signalBus;
            this.evolutionInfoBlueprint              = evolutionInfoBlueprint;
            this.elementEvolutionLocalDataController = elementEvolutionLocalDataController;
            this.inventoryLocalDataController        = inventoryLocalDataController;
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
            this.signalBus.Fire(new QuestTriggerSignal() { TriggerSignalId = QuestTriggerSignalId.UnlockHero, Value = 1 });
            this.signalBus.Fire(new QuestTriggerSignal() { TriggerSignalId = $"Unlock{heroId}", Value               = 1 });

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

        public List<string> GetPassiveSkills(string heroId)
        {
            var evolutionData = this.elementEvolutionLocalDataController.GetEvolutionElementData(heroId);
            return this.evolutionInfoBlueprint.GetDataById(evolutionData.EvolutionId).Abilities.Skip(1).ToList();
        }

        public bool UpgradeHero(string heroId, int price, int levelUpgradeAmount = 1)
        {
            if (!this.heroLocalData.IdToHeroData.TryGetValue(heroId, out var elementUpgradeData))
            {
                throw new Exception($"Not found element upgrade data of element: {heroId}");
            }

            if (!this.resourceLocalDataController.SpendResource(ResourceType.Gold, price)) return false;

            elementUpgradeData.Level += levelUpgradeAmount;
            this.signalBus.Fire(new QuestTriggerSignal() { TriggerSignalId = QuestTriggerSignalId.UpgradeHero, Value = elementUpgradeData.Level, isReset = true });
            this.signalBus.Fire(new QuestTriggerSignal() { TriggerSignalId = $"Upgrade{heroId}", Value               = elementUpgradeData.Level, isReset = true });

            return true;
        }

        public float GetStatAfterEquipItem(StatEnum statEnum, float stat, string heroId)
        {
            var heroData           = this.GetHeroLocalData(heroId);
            var equipmentStatValue = 0f;
            foreach (var equipmentId in heroData.ListEquipmentId)
            {
                var itemData = this.inventoryLocalDataController.GetItem(equipmentId);
                if (itemData == null) continue;
                equipmentStatValue += stat * itemData.GetFinalStat(statEnum,out var a) / 100;
            }

            return stat + equipmentStatValue;
        }

    }

    public class HeroRuntimeData
    {
        public HeroRecord   heroRecord;
        public float        resourceValue;
        public ResourceType resourceType;
        public string       avatar;
        public HeroStatus   heroStatus;
    }
}