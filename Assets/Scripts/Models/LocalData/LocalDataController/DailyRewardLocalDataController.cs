namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Blueprints;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Services;

    public class DailyRewardLocalDataController : ILocalDataController
    {
        private const    int                         TotalDayInWeek = 7;
        private readonly DailyRewardLocalData        dailyRewardLocalData;
        private readonly DailyRewardBlueprint        dailyRewardBlueprint;
        private readonly IInternetService            internetService;
        private readonly ResourceLocalDataController resourceLocalDataController;

        private SemaphoreSlim mySemaphoreSlim = new(1, 1);
        public DailyRewardLocalDataController(
            DailyRewardLocalData dailyRewardLocalData,
            DailyRewardBlueprint dailyRewardBlueprint,
            IInternetService internetService,
            ResourceLocalDataController resourceLocalDataController)
        {
            this.dailyRewardLocalData        = dailyRewardLocalData;
            this.dailyRewardBlueprint        = dailyRewardBlueprint;
            this.internetService             = internetService;
            this.resourceLocalDataController = resourceLocalDataController;
        }
        public void InitData() { this.InitRewardForAllDay(); }

        public List<RewardData> GetAllRewardLocalData => this.dailyRewardLocalData.RewardData;

        public RewardData GetRewardLocalData(int day) => this.dailyRewardLocalData.RewardData.First(e => e.Day == day);

        private void InitRewardForAllDay()
        {
            if (this.dailyRewardLocalData.RewardData.Count > 0) return;
            this.dailyRewardLocalData.RewardData.Clear();
            var startDay = 1;
            this.dailyRewardBlueprint.ForEach(data =>
            {
                this.dailyRewardLocalData.RewardData.Add(new RewardData() { Day = startDay, RewardStatus = RewardStatus.Lock });
                startDay++;
            });
            this.dailyRewardLocalData.RewardData.First().RewardStatus = RewardStatus.UnClaimed;
            this.dailyRewardLocalData.LastRewardedDate                = DateTime.Now;
        }

        public async UniTask CheckRewardStatus()
        {
            await this.mySemaphoreSlim.WaitAsync();

            try
            {
                var currentTime = await this.internetService.GetCurrentTimeAsync();
                //var currentTime = DateTime.Now; // Because the internet service getting time doesn't work stable I use this instead, btw, we allow hyper casual players cheat the game.
                var issDiffDay = this.internetService.IsDifferentDay(this.dailyRewardLocalData.LastRewardedDate, currentTime);

                if (!issDiffDay) return;

                var firstLockedDayIndex = this.FindFirstLockedDayIndex();

                if (firstLockedDayIndex == -1)
                {
                    if (!this.CanClaimReward)
                    {
                        this.InitRewardForAllDay();
                    }
                }
                else
                {
                    if (firstLockedDayIndex / TotalDayInWeek == (firstLockedDayIndex) / TotalDayInWeek)
                    {
                        this.dailyRewardLocalData.RewardData[firstLockedDayIndex].RewardStatus = RewardStatus.UnClaimed;
                        this.dailyRewardLocalData.LastRewardedDate                             = currentTime;
                    }
                }
            }
            finally
            {
                this.mySemaphoreSlim.Release();
            }
        }

        public void ClaimAllAvailableReward()
        {
            for (var i = 0; i < this.dailyRewardLocalData.RewardData.Count; i++)
            {
                if (this.dailyRewardLocalData.RewardData[i].RewardStatus == RewardStatus.UnClaimed)
                {
                    this.dailyRewardLocalData.RewardData[i].RewardStatus = RewardStatus.Claimed;
                    this.HandleReward(i + 1);
                }
            }
        }

        private void HandleReward(int day)
        {
            var rewardRecord = this.dailyRewardBlueprint.GetDataById(day);
            switch (rewardRecord.RewardType)
            {
                case RewardType.Resource:
                    this.resourceLocalDataController.ReceiveResource(rewardRecord.RewardId.ToEnum<ResourceType>(), rewardRecord.RewardValue);
                    break;
            }
        }
        public int FindFirstLockedDayIndex() { return this.dailyRewardLocalData.RewardData.FirstIndex(status => status.RewardStatus == RewardStatus.Lock); }

        public bool CheckCanClaim() => this.dailyRewardLocalData.RewardData.Any(e => e.RewardStatus == RewardStatus.UnClaimed);

        private bool CanClaimReward => this.dailyRewardLocalData.RewardData.Any(t => t.RewardStatus == RewardStatus.UnClaimed);
    }
}