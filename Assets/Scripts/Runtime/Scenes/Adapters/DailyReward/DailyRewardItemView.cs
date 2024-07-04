namespace Runtime.Scenes.Adapters.DailyReward
{
    using System;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class DailyRewardItemModel
    {
        public int Day;
    }
    public class DailyRewardItemView : TViewMono
    {
        public Image           rewardImage;
        public TextMeshProUGUI rewardValue;
        public GameObject      claimedBg;
        public GameObject      lockBg;
        public GameObject      readyToClaimBg;
    }
    
    public class DailyRewardItemPresenter : BaseUIItemPresenter<DailyRewardItemView,DailyRewardItemModel>
    {
        private readonly DailyRewardLocalDataController dailyRewardLocalDataController;
        private readonly DailyRewardBlueprint           rewardBlueprint;
        public           DailyRewardItemModel           model;
        public DailyRewardItemPresenter(IGameAssets gameAssets, DailyRewardLocalDataController dailyRewardLocalDataController, DailyRewardBlueprint rewardBlueprint)
            : base(gameAssets)
        {
            this.dailyRewardLocalDataController = dailyRewardLocalDataController;
            this.rewardBlueprint                = rewardBlueprint;
        }
        public override void BindData(DailyRewardItemModel param)
        {
            this.model                   = param;
            var rewardLocalData = this.dailyRewardLocalDataController.GetRewardLocalData(param.Day);
            var rewardRecord    = this.rewardBlueprint.GetDataById(param.Day);
            this.View.rewardImage.sprite = this.GameAssets.LoadAssetAsync<Sprite>(rewardRecord.RewardImage).WaitForCompletion();
            this.View.rewardValue.text   = $"{rewardRecord.RewardValue}";
            
            this.View.claimedBg.SetActive(false);
            this.View.lockBg.SetActive(false);
            this.View.readyToClaimBg.SetActive(false);
            
            switch (rewardLocalData.RewardStatus)
            {
                case RewardStatus.Lock:
                    this.View.lockBg.SetActive(true);
                    break;

                case RewardStatus.Claimed:
                    this.View.claimedBg.SetActive(true);
                    break;
                case RewardStatus.UnClaimed:
                    this.View.readyToClaimBg.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}