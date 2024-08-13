namespace Runtime.Scenes.Popups
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.LogService;
    using global::Extensions;
    using Models;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Scenes.Commons;
    using Runtime.Services;
    using Runtime.Signals;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class ConfirmEvolutionPopupView : BaseView
    {
        [SerializeField] private ElementGenericInfoView elementGenericInfoView;
        [SerializeField] private Button                 changeClassBtn,       closeBtn;
        [SerializeField] private TMP_Text               evolveDescriptionTxt, headerTxt;

        public ElementGenericInfoView ElementGenericInfoView => this.elementGenericInfoView;
        public Button                 ChangeClassBtn         => this.changeClassBtn;
        public Button                 CloseBtn               => this.closeBtn;
        public TMP_Text               EvolveDescriptionTxt   => this.evolveDescriptionTxt;
        public TMP_Text               HeaderTxt              => this.headerTxt;

        public Transform  startPos;
        public Transform  endPos;
        public GameObject viewField;
    }

    [PopupInfo(nameof(ConfirmEvolutionPopupView), isOverlay: true)]
    public class ConfirmEvolutionPopupPresenter : BasePopupPresenter<ConfirmEvolutionPopupView, ConfirmEvolutionPopupModel>
    {
        private readonly DiContainer                         diContainer;
        private readonly HeroUpgradeService                  heroUpgradeService;
        private readonly ToastController                     toastController;
        private readonly ElementEvolutionLocalDataController elementEvolutionLocalDataController;
        private readonly EvolutionBlueprint                  evolutionBlueprint;
        private readonly EvolutionInfoBlueprint              evolutionInfoBlueprint;

        public ConfirmEvolutionPopupPresenter(
            SignalBus                           signalBus,
            ILogService                         logService,
            DiContainer                         diContainer,
            HeroUpgradeService                  heroUpgradeService,
            ToastController                     toastController,
            ElementEvolutionLocalDataController elementEvolutionLocalDataController,
            EvolutionBlueprint                  evolutionBlueprint,
            EvolutionInfoBlueprint              evolutionInfoBlueprint
        )
            : base(signalBus, logService)
        {
            this.diContainer                         = diContainer;
            this.heroUpgradeService                  = heroUpgradeService;
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
            this.View.HeaderTxt.text = popupModel.ElementId;

            this.View.viewField.transform.position = this.View.startPos.position;
            this.View.viewField.transform.DOMove(this.View.endPos.position, 0.5f).SetEase(Ease.InOutQuint);

            var evolutionDes     = this.evolutionInfoBlueprint.GetDataById(popupModel.EvolutionId).EvolutionDescription;
            var passiveSkillName = evolutionDes.Split("|")[0];
            var passiveSkillDes  = evolutionDes.Split("|")[1];
            this.View.EvolveDescriptionTxt.text = passiveSkillName;
            this.View.ElementGenericInfoView.BindData(new()
            {
                ElementId   = popupModel.ElementId,
                EvolutionId = popupModel.EvolutionId,
            });
            this.View.ElementGenericInfoView.skillDescription.text = passiveSkillDes;
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
            var isReachRequireLevel = this.heroUpgradeService.GetHeroLevel(this.Model.ElementId) >= requireLevel;
            if (!isReachRequireLevel)
            {
                this.toastController.ShowToast("Level is too low");

                return;
            }

            if (this.elementEvolutionLocalDataController.UpdateEvolutionId(this.Model.ElementId, this.Model.EvolutionId))
            {
                this.toastController.ShowToast("Change class success");
                this.SignalBus.Fire<ChangeHeroClassSignal>();
                this.CloseView();
            }
            else
            {
                this.toastController.ShowToast("Not enough diamond");
            }
        }

        public override void CloseView()
        {
            this.View.viewField.transform.DOMove(this.View.startPos.position, 0.5f).SetEase(Ease.InOutQuint).onComplete += () =>
            {
                base.CloseView();
            };
        }
    }

    public class ConfirmEvolutionPopupModel
    {
        public CharacterInfoPopupModel CharacterInfoPopupModel { get; set; }
        public string                  ElementId               { get; set; }
        public string                  EvolutionId             { get; set; }
    }
}