namespace Runtime.Scenes.Popups
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using global::Extensions;
    using Models;
    using Models.LocalData.LocalDataController;
    using Runtime.Scenes.Commons;
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
        private readonly ElementEvolutionLocalDataController elementEvolutionLocalDataController;
        private readonly DiContainer                diContainer;
        private readonly EvolutionBlueprint         evolutionBlueprint;

        public ConfirmEvolutionPopupPresenter(SignalBus signalBus,
            ILogService logService,
            ElementEvolutionLocalDataController elementEvolutionLocalDataController,
            DiContainer diContainer,
            EvolutionBlueprint evolutionBlueprint)
            : base(signalBus, logService)
        {
            this.elementEvolutionLocalDataController = elementEvolutionLocalDataController;
            this.diContainer                = diContainer;
            this.evolutionBlueprint         = evolutionBlueprint;
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

        private void UpdateEvolutionId() { this.elementEvolutionLocalDataController.UpdateEvolutionId(this.Model.ElementId, this.Model.EvolutionId); }
    }

    public class ConfirmEvolutionPopupModel
    {
        public CharacterInfoPopupModel CharacterInfoPopupModel { get; set; }
        public string                  ElementId               { get; set; }
        public string                  EvolutionId             { get; set; }
    }
}