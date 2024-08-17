namespace Runtime.Scenes.Popups
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Scenes.Adapters.Talent;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;
    using R3;
    using Runtime.Enums;
    using Runtime.Services;

    public class TalentPopupView : BaseView
    {
        public GameObject      descriptionField;
        public TextMeshProUGUI description;
        public TextMeshProUGUI talentPointValue;
        public TextMeshProUGUI talentPointNeed;
        public Button          levelupBtn;
        public Button          exitBtn;
        public Image           iconTalent;
        public TalentAdapter   talentAdapter;
        public Transform       startPos;
        public Transform       endPos;
        public GameObject      viewField;
    }

    [PopupInfo(nameof(TalentPopupView), isOverlay: true)]
    public class TalentPopupPresenter : BasePopupPresenter<TalentPopupView>
    {
        private readonly TalentLocalDataController   talentLocalDataController;
        private readonly TalentBlueprint             talentBlueprint;
        private readonly DiContainer                 diContainer;
        private readonly IGameAssets                 gameAssets;
        private readonly ResourceLocalDataController resourceLocalDataController;
        private readonly ToastController             toastController;

        private TalentType currentSelectedTalent;

        public TalentPopupPresenter(
            SignalBus signalBus,
            TalentLocalDataController talentLocalDataController,
            TalentBlueprint talentBlueprint,
            DiContainer diContainer,
            IGameAssets gameAssets,
            ResourceLocalDataController resourceLocalDataController,
            ToastController toastController
        )
            : base(signalBus)
        {
            this.talentLocalDataController   = talentLocalDataController;
            this.talentBlueprint             = talentBlueprint;
            this.diContainer                 = diContainer;
            this.gameAssets                  = gameAssets;
            this.resourceLocalDataController = resourceLocalDataController;
            this.toastController             = toastController;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.levelupBtn.onClick.AddListener(this.OnUpgradeTalentClick);
            this.View.exitBtn.onClick.AddListener(this.CloseView);
            this.resourceLocalDataController.GetResource(ResourceType.TalentPoint).Subscribe(this.OnTalentPointChange);
        }

        public override async UniTask BindData()
        {
            this.View.viewField.transform.position = this.View.startPos.position;
            this.View.viewField.transform.DOMove(this.View.endPos.position, 0.5f).SetEase(Ease.OutElastic);
            var listData = this.talentLocalDataController.GetAllTalentLocalData.Select(e => new TalentItemModel()
            {
                TalentType        = e.Key,
                OnItemClickAction = this.OnItemClick
            }).ToList();
            await this.View.talentAdapter.InitItemAdapter(listData, this.diContainer);
            this.View.talentPointValue.text = $"{this.resourceLocalDataController.GetResource(ResourceType.TalentPoint).Value}";
        }

        private void OnTalentPointChange(float value)
        {
            if (this.View == null) return;
            this.View.talentPointValue.text = $"{value}";
        }

        private void OnUpgradeTalentClick()
        {
            if (this.talentLocalDataController.CheckTalentIsMaxLevel(this.currentSelectedTalent))
            {
                this.toastController.ShowToast("This talent has reached the max level");
            }
            else
            {
                var levelUpResult = this.talentLocalDataController.LevelUpTalent(this.currentSelectedTalent);
                this.toastController.ShowToast(!levelUpResult ? "Not enough talent point" : "upgrade completed");
                if (levelUpResult)
                {
                    var level       = this.talentLocalDataController.GetTalentLevel(this.currentSelectedTalent);
                    var actualLevel = this.talentLocalDataController.CheckTalentIsMaxLevel(this.currentSelectedTalent) ? level : level + 1;
                    this.View.description.text
                        = $"{this.talentBlueprint.GetDataById(this.currentSelectedTalent).Description} {this.talentBlueprint.GetDataById(this.currentSelectedTalent).TalentLevelToDataRecords[actualLevel].EffectValue}%";
                }
            }

            this.RefreshTalentInfo();
            this.View.talentAdapter.Refresh();
        }

        private void OnItemClick(TalentType talentType, int level)
        {
            var actualLevel = this.talentLocalDataController.CheckTalentIsMaxLevel(talentType) ? level : level + 1;
            this.View.descriptionField.SetActive(true);
            this.View.description.text
                = $"{this.talentBlueprint.GetDataById(talentType).Description} {this.talentBlueprint.GetDataById(talentType).TalentLevelToDataRecords[actualLevel].EffectValue}%";

            this.View.iconTalent.sprite    = this.gameAssets.LoadAssetAsync<Sprite>(this.talentBlueprint.GetDataById(talentType).Icon).WaitForCompletion();
            this.currentSelectedTalent     = talentType;
            this.View.talentPointNeed.text = $"{this.talentBlueprint.GetDataById(talentType).TalentLevelToDataRecords[actualLevel].TalentPointNeed}";
        }

        private void RefreshTalentInfo()
        {
            var level = this.talentLocalDataController.GetTalentLevel(this.currentSelectedTalent);
            var actualLevel = this.talentLocalDataController.CheckTalentIsMaxLevel(this.currentSelectedTalent) ? level : level + 1;
            this.View.description.text
                = $"{this.talentBlueprint.GetDataById(this.currentSelectedTalent).Description} {this.talentBlueprint.GetDataById(this.currentSelectedTalent).TalentLevelToDataRecords[actualLevel].EffectValue}%";
            this.View.talentPointNeed.text = $"{this.talentBlueprint.GetDataById(this.currentSelectedTalent).TalentLevelToDataRecords[actualLevel].TalentPointNeed}";
        }

        public override void CloseView() { this.View.viewField.transform.DOMove(this.View.startPos.position, 0.5f).SetEase(Ease.InOutQuint).onComplete += () => { base.CloseView(); }; }
    }
}