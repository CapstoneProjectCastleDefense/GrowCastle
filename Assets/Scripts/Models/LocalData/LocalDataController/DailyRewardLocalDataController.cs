namespace Models.LocalData.LocalDataController
{
    using System;
    using System.Linq;
    using System.Threading;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Blueprints;
    using Runtime.Services;

    public class DailyRewardLocalDataController : ILocalDataController
    {
        private const    int                  TotalDayInWeek = 7;
        private readonly DailyRewardLocalData dailyRewardLocalData;
        private readonly DailyRewardBlueprint dailyRewardBlueprint;
        private readonly IInternetService      internetService;

        private SemaphoreSlim mySemaphoreSlim = new(1, 1);
        public DailyRewardLocalDataController(DailyRewardLocalData dailyRewardLocalData, DailyRewardBlueprint dailyRewardBlueprint, IInternetService internetService)
        {
            this.dailyRewardLocalData = dailyRewardLocalData;
            this.dailyRewardBlueprint = dailyRewardBlueprint;
            this.internetService      = internetService;
        }
        public void InitData()
        {
            this.InitRewardForAllDay();
        }

        private void InitRewardForAllDay()
        {
            this.dailyRewardLocalData.RewardData.Clear();
            this.dailyRewardBlueprint.ForEach(data =>
            {
                this.dailyRewardLocalData.RewardData.Add(new RewardData(){RewardId = data.Value.RewardId,RewardStatus = RewardStatus.Lock});
            });
            this.dailyRewardLocalData.RewardData.First().RewardStatus = RewardStatus.UnClaimed;
            this.dailyRewardLocalData.LastRewardedDate = DateTime.Now;
        }
        
        public async UniTask CheckRewardStatus()
        {
            await this.mySemaphoreSlim.WaitAsync();

            try
            {
                // var currentTime = await this.internetService.GetCurrentTimeAsync();
                var currentTime = DateTime.Now; // Because the internet service getting time doesn't work stable I use this instead, btw, we allow hyper casual players cheat the game.
                var issDiffDay  = this.internetService.IsDifferentDay(this.dailyRewardLocalData.LastRewardedDate, currentTime);

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
                        this.dailyRewardLocalData.LastRewardedDate                        = currentTime;
                    }
                }
            }
            finally
            {
                this.mySemaphoreSlim.Release();
            }
        }
        private int FindFirstLockedDayIndex() { return this.dailyRewardLocalData.RewardData.FirstIndex(status => status.RewardStatus == RewardStatus.Lock); }

        private bool CanClaimReward => this.dailyRewardLocalData.RewardData.Any(t => t.RewardStatus == RewardStatus.UnClaimed);

    }
}