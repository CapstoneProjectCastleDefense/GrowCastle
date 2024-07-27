namespace Runtime.Scenes.Popups
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Models.LocalData.LocalDataController;
    using Runtime.Scenes.Commons;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class ConfirmEvolutionPopupView : BaseView
    {
        [SerializeField] private ElementGenericInfoView elementGenericInfoView;
        [SerializeField] private Button                 changeClassBtn;

        public ElementGenericInfoView ElementGenericInfoView => this.elementGenericInfoView;
        public Button                 ChangeClassBtn         => this.changeClassBtn;
    }

    [PopupInfo(nameof(ConfirmEvolutionPopupView), isOverlay: true)]
    public class ConfirmEvolutionPopupPresenter : BasePopupPresenter<ConfirmEvolutionPopupView, ConfirmEvolutionPopupModel>
    {
        private readonly ElementLocalDataController elementLocalDataController;
        private readonly DiContainer                diContainer;

        public ConfirmEvolutionPopupPresenter(SignalBus signalBus,
            ILogService logService,
            ElementLocalDataController elementLocalDataController,
            DiContainer diContainer)
            : base(signalBus, logService)
        {
            this.elementLocalDataController = elementLocalDataController;
            this.diContainer                = diContainer;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.ChangeClassBtn.onClick.AddListener(this.UpdateEvolutionId);
            this.diContainer.InjectGameObject(this.View.ElementGenericInfoView.gameObject);
        }

        public override UniTask BindData(ConfirmEvolutionPopupModel popupModel)
        {
            this.View.ElementGenericInfoView.BindData(new ElementGenericInfoModel()
            {
                ElementId   = popupModel.ElementId,
                EvolutionId = popupModel.EvolutionId
            });
            return UniTask.CompletedTask;
        }

        private void UpdateEvolutionId() { this.elementLocalDataController.UpdateEvolutionId(this.Model.ElementId, this.Model.EvolutionId); }
    }

    public class ConfirmEvolutionPopupModel
    {
        public CharacterInfoPopupModel CharacterInfoPopupModel { get; set; }
        public string                  ElementId               { get; set; }
        public string                  EvolutionId             { get; set; }
    }
}