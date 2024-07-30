namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Blueprints;
    using R3;
    using Runtime.Services;

    public class QuestLocalDataController : ILocalDataController
    {
        private readonly QuestBlueprint              questBlueprint;
        private readonly QuestLocalData              questLocalData;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly InternetService             internetService;
        private          SemaphoreSlim               mySemaphoreSlim = new(1, 1);
        public QuestLocalDataController(QuestBlueprint questBlueprint, QuestLocalData questLocalData, ResourceLocalDataController resourceLocalDataController, InternetService internetService)
        {
            this.questBlueprint              = questBlueprint;
            this.questLocalData              = questLocalData;
            this.resourceLocalDataController = resourceLocalDataController;
            this.internetService             = internetService;
        }
        public async void InitData()
        {
            if (this.questLocalData.AllQuestData.Count == 0)
            {
                this.questBlueprint.ForEach(quest =>
                {
                    this.questLocalData.AllQuestData.Add(quest.Key, new QuestData() { QuestId = quest.Key, CurrentValue = new ReactiveProperty<float>(0), QuestStatus = QuestStatus.Inprogress });
                });
                this.questLocalData.LastDate = DateTime.Now;
                return;
            }

            await this.UpdateAllQuestByDay();
        }

        private async UniTask UpdateAllQuestByDay()
        {
            await this.mySemaphoreSlim.WaitAsync();

            try
            {
                var currentTime = await this.internetService.GetCurrentTimeAsync();
                var issDiffDay  = this.internetService.IsDifferentDay(this.questLocalData.LastDate, currentTime);

                if (!issDiffDay) return;
                this.GetAllQuestWithType(QuestType.Daily).ForEach(e =>
                {
                    e.CurrentValue.Value = 0;
                    e.QuestStatus        = QuestStatus.Inprogress;
                });
                if (this.internetService.ToTalDiffDay(this.questLocalData.LastDate, currentTime) >= 7)
                {
                    this.GetAllQuestWithType(QuestType.Weekly).ForEach(e =>
                    {
                        e.CurrentValue.Value = 0;
                        e.QuestStatus        = QuestStatus.Inprogress;
                    });
                    this.questLocalData.LastDate = currentTime;
                }
            }
            finally
            {
                this.mySemaphoreSlim.Release();
            }
        }

        public List<QuestData> GetAllQuestHaveTriggerSignal(string triggerSignalId)
        {
            return this.questLocalData.AllQuestData.Where(quest => this.questBlueprint.GetDataById(quest.Key).SignalTriggerId.Equals(triggerSignalId)).Select(e => e.Value).ToList();
        }

        public List<QuestData> GetAllQuestWithType(QuestType questType)
        {
            return this.questLocalData.AllQuestData.Values.Where(quest => this.questBlueprint.GetDataById(quest.QuestId).QuestType == questType).ToList();
        }

        public QuestData GetQuestLocalData(string questId) => this.questLocalData.AllQuestData[questId];

        public void UpdateQuestProgress(string questId, float value)
        {
            var questData = this.questLocalData.AllQuestData[questId];
            if (questData.QuestStatus == QuestStatus.Complete) return;
            var currentQuestValue = questData.CurrentValue.Value;
            currentQuestValue += value;
            if (currentQuestValue >= this.questBlueprint.GetDataById(questId).TargetValue)
            {
                currentQuestValue     = this.questBlueprint.GetDataById(questId).TargetValue;
                questData.QuestStatus = QuestStatus.Complete;
            }

            questData.CurrentValue.Value = currentQuestValue;
        }

        public void ClaimQuestReward(string questId)
        {
            var rewardType = this.questBlueprint.GetDataById(questId).RewardType;
            this.resourceLocalDataController.ReceiveResource(rewardType, this.questBlueprint.GetDataById(questId).RewardValue);
            this.GetQuestLocalData(questId).QuestStatus = QuestStatus.Claimed;
        }
    }
}