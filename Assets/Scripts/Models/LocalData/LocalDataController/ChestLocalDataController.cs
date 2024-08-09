namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Blueprints;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Signals.Quests;
    using Runtime.StaticValues;
    using Sirenix.Utilities;
    using Zenject;

    public class ChestLocalDataController : ILocalDataController
    {
        private readonly ChestBlueprint               chestBlueprint;
        private readonly ChestLocalData               chestLocalData;
        private readonly ResourceLocalDataController  resourceLocalDataController;
        private readonly SignalBus                    signalBus;
        private readonly InventoryLocalDataController inventoryLocalDataController;
        private readonly ItemBlueprint                itemBlueprint;

        public ChestLocalDataController(ChestBlueprint chestBlueprint, ChestLocalData chestLocalData, ResourceLocalDataController resourceLocalDataController, SignalBus signalBus, InventoryLocalDataController inventoryLocalDataController, ItemBlueprint itemBlueprint)
        {
            this.chestBlueprint               = chestBlueprint;
            this.chestLocalData               = chestLocalData;
            this.resourceLocalDataController  = resourceLocalDataController;
            this.signalBus                    = signalBus;
            this.inventoryLocalDataController = inventoryLocalDataController;
            this.itemBlueprint                = itemBlueprint;
        }

        public void InitData()
        {
            if (this.chestLocalData.IsInit)
            {
                this.chestLocalData.ChestData.ForEach(chest =>
                {
                    chest.ChestRecord = this.chestBlueprint.GetDataById(chest.ChestType);
                });

                return;
            }

            this.chestLocalData.ChestData = new();
            this.chestBlueprint.ForEach(e =>
            {
                this.chestLocalData.ChestData.Add(new ChestData() { ChestType = e.Key, ChestRecord = e.Value });
            });
            this.chestLocalData.IsInit = true;
        }

        public List<ChestData> GetAllChestLocalData() => this.chestLocalData.ChestData;

        public ChestLocalData GetChestLocalData() => this.chestLocalData;

        public ChestData GetChestData(ResourceType chestType) => this.chestLocalData.ChestData.First(e => e.ChestType == chestType);

        public void ReceiveChest(ResourceType chestType) { this.chestLocalData.ChestData.Add(new ChestData() { ChestType = chestType, ChestRecord = this.chestBlueprint.GetDataById(chestType)}); }

        public List<PoolItem> OpenChest(ResourceType chestType)
        {
            if (this.chestLocalData.ChestData.All(e => e.ChestType != chestType)) return null;
            var            chestData = this.chestLocalData.ChestData.First(e => e.ChestType == chestType);
            var            poolItems = chestData.ChestRecord.PoolItems.ToList();
            List<PoolItem> result    = poolItems.RandomGachaWithWeight(poolItems.Select(e => e.Weight).ToList(), chestData.ChestRecord.ItemQuantity, 0);
            result.ForEach(item =>
            {
                if (item.ItemId.IsStringInEnum<ResourceType>())
                {
                    this.resourceLocalDataController.ReceiveResource(item.ItemId.ToEnum<ResourceType>(), item.Value);
                }
                else
                {
                    var itemId     = item.ItemId.Split("|")[0];
                    var rarity     = item.ItemId.Split("|")[1].ToEnum<RarityEnum>();
                    var itemRecord = this.itemBlueprint.GetDataById(itemId);
                    this.inventoryLocalDataController.AddItem(itemRecord.Id,item.Value,rarity,false,1,1,this.inventoryLocalDataController.GetStatOfItem(rarity));
                }
            });
            this.chestLocalData.ChestData.Remove(chestData);
            this.signalBus.Fire(new QuestTriggerSignal(){TriggerSignalId = QuestTriggerSignalId.OpenChest, Value = 1});
            this.signalBus.Fire(new QuestTriggerSignal(){TriggerSignalId = $"Open{chestType.ToString()}", Value = 1});
            return result;
        }
    }
}