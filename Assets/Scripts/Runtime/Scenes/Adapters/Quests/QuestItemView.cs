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
        public GameObject      claimed;
        public Image           progressBar;
    }
    
    public class QuestItemPresenter: BaseUIItemPresenter<QuestItemView,QuestItemModel>
    {
        private readonly QuestBlueprint           questBlueprint;
        private readonly QuestLocalDataController questLocalDataController;
        private          QuestItemModel           model;
        public QuestItemPresenter(IGameAssets gameAssets, QuestBlueprint questBlueprint, QuestLocalDataController questLocalDataController)
            : base(gameAssets)
        {
            this.questBlueprint           = questBlueprint;
            this.questLocalDataController = questLocalDataController;
        }

        public override void OnViewReady()
        {
            base.OnViewReady();
            this.View.claimButton.onClick.AddListener(this.OnClaimButtonClick);
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
            
            questData.CurrentValue.Dispose();
            questData.CurrentValue.Subscribe(this.OnCurrentValueChange);
            this.View.progressBar.fillAmount = questData.CurrentValue.Value / questRecord.TargetValue;
            
            this.View.claimed.SetActive(false);
            this.View.inprogress.SetActive(false);
            this.View.claimButton.gameObject.SetActive(false);
            switch (questData.QuestStatus)
            {
                case QuestStatus.Claimed:
                    this.View.claimed.SetActive(true);
                    break;
                case QuestStatus.Complete:
                    this.View.claimButton.gameObject.SetActive(true);
                    break;
                case QuestStatus.Inprogress:
                    this.View.inprogress.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void OnCurrentValueChange(float value)
        {
            var questRecord = this.questBlueprint.GetDataById(this.model.QuestId);
            this.View.currentValue.text      = $"{value}";
            this.View.progressBar.fillAmount = value / questRecord.TargetValue;
        }

        private void OnClaimButtonClick()
        {
            this.questLocalDataController.ClaimQuestReward(this.model.QuestId);
            this.BindData(this.model);
        }
    }
}