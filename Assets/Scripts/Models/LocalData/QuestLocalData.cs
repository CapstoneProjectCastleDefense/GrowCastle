namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;
    using R3;

    public class QuestLocalData : ILocalDataHaveController<QuestLocalDataController>
    {
        public Dictionary<string, QuestData> AllQuestData = new();
        public void                          Init() { }
    }

    public class QuestData
    {
        public string                  QuestId      { get; set; }
        public ReactiveProperty<float> CurrentValue { get; set; }
        public QuestStatus             QuestStatus  { get; set; }
    }

    public enum QuestStatus
    {
        Inprogress,
        Complete,
        Claimed
    }
}