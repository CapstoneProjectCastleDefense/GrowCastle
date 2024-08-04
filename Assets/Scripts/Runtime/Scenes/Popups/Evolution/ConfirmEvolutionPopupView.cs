namespace Runtime.Scenes.Popups
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using global::Extensions;
    using Models;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Scenes.Commons;
    using Runtime.Services;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class ConfirmEvolutionPopupView : BaseView
    {
        [SerializeField] private ElementGenericInfoView elementGenericInfoView;
        [SerializeField] private Button                 changeClassBtn, closeBtn;

        public ElementGenericInfoView ElementGenericInfoView => this.elementGenericInfoView;
        public Button                 ChangeClassBtn         => this.changeClassBtn;
        public Button                 CloseBtn               => this.closeBtn;
    }

    [PopupInfo(nameof(ConfirmEvolutionPopupView), isOverlay: true)]
    public class ConfirmEvolutionPopupPresenter : BasePopupPresenter<ConfirmEvolutionPopupView, ConfirmEvolutionPopupModel>
    {
        private readonly DiContainer                         diContainer;
        private readonly ElementUpgradeService               elementUpgradeService;
        private readonly ToastController                     toastController;
        private readonly ElementEvolutionLocalDataController elementEvolutionLocalDataController;
        private readonly EvolutionBlueprint                  evolutionBlueprint;
        private readonly EvolutionInfoBlueprint              evolutionInfoBlueprint;

        public ConfirmEvolutionPopupPresenter(SignalBus signalBus,
            ILogService logService,
            DiContainer diContainer,
            ElementUpgradeService elementUpgradeService,
            ToastController toastController,
            ElementEvolutionLocalDataController elementEvolutionLocalDataController,
            EvolutionBlueprint evolutionBlueprint,
            EvolutionInfoBlueprint evolutionInfoBlueprint)
            : base(signalBus, logService)
        {
            this.diContainer                         = diContainer;
            this.elementUpgradeService               = elementUpgradeService;
            this.toastController                     = toastController;
            this.elementEvolutionLocalDataController = elementEvolutionLocalDataController;
            this.evolutionBlueprint                  = evolutionBlueprint;
            this.evolutionInfoBlueprint              = evolutionInfoBlueprint;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.ChangeClassBtn.onClick.AddListener(this.UpdateEvolutionId);
            this.View.CloseBtn.onClick.AddListener(this.CloseView);
            this.diContainer.InjectGameObject(this.View.ElementGenericInfoView.gameObject);
        }

        public override UniTask BindData(ConfirmEvolutionPopupModel popupModel)
        {
            this.View.ElementGenericInfoView.BindData(new ElementGenericInfoModel()
            {
                ElementId   = popupModel.ElementId,
                EvolutionId = popupModel.EvolutionId
            });

            var evolutionLocalData    = this.elementEvolutionLocalDataController.GetEvolutionElementData(popupModel.ElementId);
            var evolutionDetailRecord = this.evolutionBlueprint.GetEvolutionDetailRecord(popupModel.ElementId, popupModel.EvolutionId);
            var parentId              = evolutionDetailRecord.ParentId;
            var canChangeClass        = parentId.IsNullOrEmpty() || evolutionLocalData.OwnedEvolutions.Contains(parentId);
            this.View.ChangeClassBtn.gameObject.SetActive(canChangeClass);

            return UniTask.CompletedTask;
        }

        private void UpdateEvolutionId()
        {
            var requireLevel        = this.evolutionInfoBlueprint.GetDataById(this.Model.EvolutionId).RequireLevel;
            var isReachRequireLevel = this.elementUpgradeService.GetElementLevel(this.Model.ElementId) >= requireLevel;
            if (!isReachRequireLevel)
            {
                this.toastController.ShowToast("Level is too low");
                return;
            }
            
            this.elementEvolutionLocalDataController.UpdateEvolutionId(this.Model.ElementId, this.Model.EvolutionId);
        }
    }

    public class ConfirmEvolutionPopupModel
    {
        public CharacterInfoPopupModel CharacterInfoPopupModel { get; set; }
        public string                  ElementId               { get; set; }
        public string                  EvolutionId             { get; set; }
    }
}