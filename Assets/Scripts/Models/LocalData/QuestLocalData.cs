namespace Models.LocalData
{
    using System;
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;
    using R3;
    using Sirenix.Serialization;

    public class QuestLocalData : ILocalDataHaveController<QuestLocalDataController>
    {
        public                 Dictionary<string, QuestData> AllQuestData = new();
        [OdinSerialize] public DateTime                      LastDate { get; set; }
        public                 void                          Init()           { }
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