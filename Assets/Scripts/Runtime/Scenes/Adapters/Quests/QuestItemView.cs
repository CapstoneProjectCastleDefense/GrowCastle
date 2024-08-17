namespace Runtime.Scenes.Adapters.Quests
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
    using R3;
    using UnityEngine.Serialization;

    public class QuestItemModel
    {
        public string QuestId;
    }

    public class QuestItemView : TViewMono
    {
        public Image           questIcon;
        public TextMeshProUGUI questDescription;
        public TextMeshProUGUI currentValue;
        public TextMeshProUGUI targetValue;
        public Button          claimButton;
        public GameObject      inprogress;
        public GameObject      completedText;
        public GameObject      progressField;
        public Image           progressBar;
        public Image           rewardImage;
        public TextMeshProUGUI rewardValue;
    }

    public class QuestItemPresenter : BaseUIItemPresenter<QuestItemView, QuestItemModel>
    {
        private readonly QuestBlueprint           questBlueprint;
        private readonly QuestLocalDataController questLocalDataController;
        private readonly ResourceBlueprint        resourceBlueprint;
        private          QuestItemModel           model;

        public QuestItemPresenter(IGameAssets gameAssets, QuestBlueprint questBlueprint, QuestLocalDataController questLocalDataController, ResourceBlueprint resourceBlueprint)
            : base(gameAssets)
        {
            this.questBlueprint           = questBlueprint;
            this.questLocalDataController = questLocalDataController;
            this.resourceBlueprint        = resourceBlueprint;
        }

        public override void BindData(QuestItemModel param)
        {
            this.model = param;
            var questRecord = this.questBlueprint.GetDataById(this.model.QuestId);
            var questData   = this.questLocalDataController.GetQuestLocalData(this.model.QuestId);
            this.View.questIcon.sprite      = this.GameAssets.LoadAssetAsync<Sprite>(questRecord.QuestIcon).WaitForCompletion();
            this.View.questDescription.text = questRecord.Description;
            this.View.targetValue.text      = $"{questRecord.TargetValue}";
            this.View.currentValue.text     = $"{questData.CurrentValue}";
            this.View.claimButton.onClick.RemoveAllListeners();
            this.View.claimButton.onClick.AddListener(this.OnClaimButtonClick);

            this.View.rewardImage.sprite = this.GameAssets.LoadAssetAsync<Sprite>(this.resourceBlueprint.GetDataById(questRecord.RewardType).Image).WaitForCompletion();
            this.View.rewardValue.text   = $"{questRecord.RewardValue}";

            questData.CurrentValue.Subscribe(this.OnCurrentValueChange);
            this.View.progressBar.fillAmount = questData.CurrentValue.Value / questRecord.TargetValue;

            this.View.completedText.SetActive(false);
            this.View.inprogress.SetActive(false);
            this.View.claimButton.gameObject.SetActive(false);
            switch (questData.QuestStatus)
            {
                case QuestStatus.Claimed:
                    this.View.completedText.SetActive(true);
                    this.View.progressField.SetActive(false);
                    this.View.claimButton.gameObject.SetActive(false);
                    break;
                case QuestStatus.Complete:
                    this.View.claimButton.gameObject.SetActive(true);
                    this.View.progressField.SetActive(true);
                    this.View.completedText.SetActive(false);
                    break;
                case QuestStatus.Inprogress:
                    this.View.progressField.SetActive(true);
                    this.View.claimButton.gameObject.SetActive(false);
                    this.View.completedText.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnCurrentValueChange(float value)
        {
            if (this.View == null) return;
            var questRecord = this.questBlueprint.GetDataById(this.model.QuestId);
            this.View.currentValue.text      = $"{value}";
            this.View.progressBar.fillAmount = value / questRecord.TargetValue;
            if (!(Math.Abs(this.View.progressBar.fillAmount - 1) < 0.0001f)) return;
            this.View.claimButton.gameObject.SetActive(true);
            this.View.progressField.SetActive(true);
            this.View.completedText.SetActive(false);
        }

        private void OnClaimButtonClick()
        {
            this.questLocalDataController.ClaimQuestReward(this.model.QuestId);
            this.BindData(this.model);
        }
    }
}