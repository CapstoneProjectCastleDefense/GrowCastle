namespace Runtime.Scenes.Popups
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models;
    using Runtime.Scenes.Commons;
    using Runtime.Signals;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class ElementEvolvePopupView : BaseView
    {
        [SerializeField] private EvolveItemUI     evolveItemUI;
        [SerializeField] private List<GameObject> levelPos;
        [SerializeField] private List<GameObject> linePos;
        [SerializeField] private Button           closeButton;
        [SerializeField] private GameObject       evolveItemUIContainer;

        public EvolveItemUI     EvolveItemUI          => this.evolveItemUI;
        public List<GameObject> LevelPos              => this.levelPos;
        public List<GameObject> LinePos               => this.linePos;
        public Button           CloseButton           => this.closeButton;
        public GameObject       EvolveItemUIContainer => this.evolveItemUIContainer;
    }

    [PopupInfo(nameof(ElementEvolvePopupView), isOverlay: true)]
    public class ElementEvolvePopupPresenter : BasePopupPresenter<ElementEvolvePopupView, ElementEvolvePopupModel>
    {
        private readonly EvolutionBlueprint evolutionBlueprint;
        private readonly ObjectPoolManager  objectPoolManager;
        private readonly DiContainer        diContainer;

        public ElementEvolvePopupPresenter(SignalBus signalBus,
            ILogService logService,
            EvolutionBlueprint evolutionBlueprint,
            ObjectPoolManager objectPoolManager,
            DiContainer diContainer)
            : base(signalBus, logService)
        {
            this.evolutionBlueprint = evolutionBlueprint;
            this.objectPoolManager  = objectPoolManager;
            this.diContainer        = diContainer;
        }

        private List<EvolveItemUI> evolveItemUIs = new();

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.CloseButton.onClick.AddListener(this.CloseView);
            this.SignalBus.Subscribe<ChangeHeroClassSignal>(this.OnHeroClassChange);
        }

        private void OnHeroClassChange(ChangeHeroClassSignal signal)
        {
            this.BindData(this.Model);
        }

        public override UniTask BindData(ElementEvolvePopupModel popupModel)
        {
            var evolutionRecord = this.evolutionBlueprint[popupModel.CharacterId];
            foreach (var (level, record) in evolutionRecord.LevelToEvolutionDetailRecords)
            {
                foreach (var (evolutionId, _) in record.EvolutionDetailRecords)
                {
                    var evolveItemUI = this.objectPoolManager.Spawn(this.View.EvolveItemUI, this.View.EvolveItemUIContainer.transform);
                    this.diContainer.InjectGameObject(evolveItemUI.gameObject);
                    evolveItemUI.BindData(new EvolveItemUIModel
                    {
                        ElementId   = this.Model.CharacterId,
                        EvolutionId = evolutionId,
                        LevelPos    = this.View.LevelPos[level - 1].transform.position,
                        LinePos     = this.View.LinePos
                    });
                    this.evolveItemUIs.Add(evolveItemUI);
                }
            }

            return UniTask.CompletedTask;
        }

        public override void Dispose()
        {
            base.Dispose();
            this.evolveItemUIs.ForEach(item => item.Dispose());
            this.evolveItemUIs.Clear();
        }
    }

    public class ElementEvolvePopupModel
    {
        public string CharacterId;
    }
}