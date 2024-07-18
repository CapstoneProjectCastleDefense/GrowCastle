namespace Runtime.Scenes.Popups
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
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

    public class TalentPopupView : BaseView
    {
        public GameObject      descriptionField;
        public TextMeshProUGUI description;
        public TextMeshProUGUI talentPointValue;
        public Button          levelupBtn;
        public Button          exitBtn;
        public Image           iconTalent;
        public TalentAdapter   talentAdapter;
    }

    [PopupInfo(nameof(TalentPopupView), isOverlay: true)]
    public class TalentPopupPresenter : BasePopupPresenter<TalentPopupView>
    {
        private readonly TalentLocalDataController   talentLocalDataController;
        private readonly TalentBlueprint             talentBlueprint;
        private readonly DiContainer                 diContainer;
        private readonly IGameAssets                 gameAssets;
        private readonly ResourceLocalDataController resourceLocalDataController;

        private TalentType currentSelectedTalent;
        public TalentPopupPresenter(
            SignalBus signalBus,
            TalentLocalDataController talentLocalDataController,
            TalentBlueprint talentBlueprint,
            DiContainer diContainer,
            IGameAssets gameAssets,
            ResourceLocalDataController resourceLocalDataController)
            : base(signalBus)
        {
            this.talentLocalDataController   = talentLocalDataController;
            this.talentBlueprint             = talentBlueprint;
            this.diContainer                 = diContainer;
            this.gameAssets                  = gameAssets;
            this.resourceLocalDataController = resourceLocalDataController;
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
            if (!this.talentLocalDataController.LevelupTalent(this.currentSelectedTalent)) return;
            if (this.talentLocalDataController.CheckTalentIsMaxLevel(this.currentSelectedTalent))
            {
                this.View.talentAdapter.Refresh();
                return;
            }

            var level = this.talentLocalDataController.GetTalentLevel(this.currentSelectedTalent);
            this.View.description.text
                = $"{this.talentBlueprint.GetDataById(this.currentSelectedTalent).Description} {this.talentBlueprint.GetDataById(this.currentSelectedTalent).TalentLevelToDataRecords[level + 1].EffectValue}";
            this.View.talentAdapter.Refresh();
        }

        private void OnItemClick(TalentType talentType, int level)
        {
            this.View.descriptionField.SetActive(true);
            if (!this.talentLocalDataController.CheckTalentIsMaxLevel(talentType))
            {
                this.View.description.text
                    = $"{this.talentBlueprint.GetDataById(talentType).Description} {this.talentBlueprint.GetDataById(talentType).TalentLevelToDataRecords[level + 1].EffectValue}";
            }

            this.View.iconTalent.sprite = this.gameAssets.LoadAssetAsync<Sprite>(this.talentBlueprint.GetDataById(talentType).Icon).WaitForCompletion();
            this.currentSelectedTalent  = talentType;
        }
    }
}