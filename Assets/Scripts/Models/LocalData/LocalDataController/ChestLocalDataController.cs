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
        private readonly ChestBlueprint              chestBlueprint;
        private readonly ChestLocalData              chestLocalData;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly SignalBus                   signalBus;

        public ChestLocalDataController(ChestBlueprint chestBlueprint, ChestLocalData chestLocalData, ResourceLocalDataController resourceLocalDataController, SignalBus signalBus)
        {
            this.chestBlueprint              = chestBlueprint;
            this.chestLocalData              = chestLocalData;
            this.resourceLocalDataController = resourceLocalDataController;
            this.signalBus                   = signalBus;
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

        public void ReceiveChest(ResourceType chestType) { this.chestLocalData.ChestData.Add(new ChestData() { ChestType = chestType }); }

        public List<PoolItem> OpenChest(ResourceType chestType)
        {
            if (this.chestLocalData.ChestData.All(e => e.ChestType != chestType)) return null;
            var            chestData = this.chestLocalData.ChestData.First(e => e.ChestType == chestType);
            var            poolItems = chestData.ChestRecord.PoolItems.ToList();
            List<PoolItem> result    = poolItems.RandomGachaWithWeight(poolItems.Select(e => e.Weight).ToList(), chestData.ChestRecord.ItemQuantity, 0);
            poolItems.ForEach(item =>
            {
                this.resourceLocalDataController.ReceiveResource(item.ItemType, item.Value);
            });
            this.chestLocalData.ChestData.Remove(chestData);
            this.signalBus.Fire(new QuestTriggerSignal(){TriggerSignalId = QuestTriggerSignalId.OpenChest, Value = 1});
            this.signalBus.Fire(new QuestTriggerSignal(){TriggerSignalId = $"Open{chestType.ToString()}", Value = 1});
            return result;
        }
    }
}