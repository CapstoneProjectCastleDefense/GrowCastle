namespace Runtime.Systems.Quests
{
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Signals.Quests;
    using Zenject;

    public class HandleQuestSystem : IGameSystem
    {
        private readonly QuestLocalDataController questLocalDataController;
        private readonly QuestBlueprint           questBlueprint;
        private readonly SignalBus                signalBus;
        public HandleQuestSystem(QuestLocalDataController questLocalDataController, QuestBlueprint questBlueprint, SignalBus signalBus)
        {
            this.questLocalDataController = questLocalDataController;
            this.questBlueprint           = questBlueprint;
            this.signalBus                = signalBus;
        }
        public void Initialize()
        {
           this.signalBus.Subscribe<QuestTriggerSignal>(this.HandleUpdateQuestData);
        }

        private void HandleUpdateQuestData(QuestTriggerSignal signal)
        {
            var questNeedToUpdate = this.questLocalDataController.GetAllQuestHaveTriggerSignal(signal.TriggerSignalId);
            questNeedToUpdate.ForEach(quest =>
            {
                this.questLocalDataController.UpdateQuestProgress(quest.QuestId,signal.Value);
            });
        }
        public void Tick()
        {
            
        }
        public void Dispose()
        {
            this.signalBus.Unsubscribe<QuestTriggerSignal>(this.HandleUpdateQuestData);
        }
    }
}