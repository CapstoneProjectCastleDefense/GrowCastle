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

    public class TalentPopupView : BaseView
    {
        public GameObject      descriptionField;
        public TextMeshProUGUI description;
        public Button          levelupBtn;
        public Button          exitBtn;
        public Image           iconTalent;
        public TalentAdapter   talentAdapter;
    }

    [PopupInfo(nameof(TalentPopupView),isOverlay: true)]
    public class TalentPopupPresenter : BasePopupPresenter<TalentPopupView>
    {
        private readonly TalentLocalDataController talentLocalDataController;
        private readonly TalentBlueprint           talentBlueprint;
        private readonly DiContainer               diContainer;
        private readonly IGameAssets               gameAssets;

        private TalentType currentSelectedTalent;
        public TalentPopupPresenter(SignalBus signalBus, TalentLocalDataController talentLocalDataController, TalentBlueprint talentBlueprint, DiContainer diContainer, IGameAssets gameAssets)
            : base(signalBus)
        {
            this.talentLocalDataController = talentLocalDataController;
            this.talentBlueprint           = talentBlueprint;
            this.diContainer               = diContainer;
            this.gameAssets                = gameAssets;
        }
        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.levelupBtn.onClick.AddListener(this.OnUpgradeTalentClick);
            this.View.exitBtn.onClick.AddListener(this.CloseView);
        }
        public override async UniTask BindData()
        {
            var listData = this.talentLocalDataController.GetAllTalentLocalData.Select(e => new TalentItemModel()
            {
                TalentType = e.Key,
                OnItemClickAction = this.OnItemClick
            }).ToList();
            await this.View.talentAdapter.InitItemAdapter(listData, this.diContainer);
        }

        private void OnUpgradeTalentClick()
        {
            this.talentLocalDataController.LevelupTalent(this.currentSelectedTalent);
            this.View.talentAdapter.Refresh();
        }

        private void OnItemClick(TalentType talentType, int level)
        {
            this.View.descriptionField.SetActive(true);
            this.View.description.text  = $"{this.talentBlueprint.GetDataById(talentType).Description} {this.talentBlueprint.GetDataById(talentType).TalentLevelToDataRecords[level].EffectValue}";
            this.View.iconTalent.sprite = this.gameAssets.LoadAssetAsync<Sprite>(this.talentBlueprint.GetDataById(talentType).Icon).WaitForCompletion();
            this.currentSelectedTalent  = talentType;
            
        }
    }
}