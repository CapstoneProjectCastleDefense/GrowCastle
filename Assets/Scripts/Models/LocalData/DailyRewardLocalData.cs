namespace Models.LocalData
{
    using System;
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;
    using Sirenix.Serialization;

    public class DailyRewardLocalData : ILocalDataHaveController<DailyRewardLocalDataController>
    {
        public List<RewardData> RewardData = new();
        [OdinSerialize] public DateTime LastRewardedDate    { get; set; }
        [OdinSerialize] public DateTime FirstTimeOpenedDate { get; set; } = DateTime.Now;
        public void Init()
        {
            
        }
    }

    public class RewardData
    {
        public int          Day;
        public RewardStatus RewardStatus;
    }

    public enum RewardStatus
    {
        Lock,
        Claimed,
        UnClaimed
    }
}