namespace Runtime.Scenes.Popups
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models;
    using Runtime.Scenes.Commons;
    using Runtime.Signals;
    using TMPro;
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
        [SerializeField] private TMP_Text         headerTxt;

        public EvolveItemUI     EvolveItemUI          => this.evolveItemUI;
        public List<GameObject> LevelPos              => this.levelPos;
        public List<GameObject> LinePos               => this.linePos;
        public Button           CloseButton           => this.closeButton;
        public GameObject       EvolveItemUIContainer => this.evolveItemUIContainer;
        public TMP_Text         HeaderTxt             => this.headerTxt;
        
        public Transform  startPos;
        public Transform  endPos;
        public GameObject viewField;
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
            this.Rebind();
        }

        public override UniTask BindData(ElementEvolvePopupModel popupModel)
        {
            this.View.viewField.transform.position = this.View.startPos.position;
            this.View.viewField.transform.DOMove(this.View.endPos.position, 0.5f).SetEase(Ease.InOutQuint);
            this.View.HeaderTxt.text = $"{popupModel.ElementId}";
            
            var evolutionRecord = this.evolutionBlueprint[popupModel.ElementId];
            foreach (var (level, record) in evolutionRecord.LevelToEvolutionDetailRecords)
            {
                foreach (var (evolutionId, _) in record.EvolutionDetailRecords)
                {
                    var evolveItemUI = this.objectPoolManager.Spawn(this.View.EvolveItemUI, this.View.EvolveItemUIContainer.transform);
                    this.diContainer.InjectGameObject(evolveItemUI.gameObject);
                    evolveItemUI.BindData(new EvolveItemUIModel
                    {
                        ElementId   = this.Model.ElementId,
                        EvolutionId = evolutionId,
                        LevelPos    = this.View.LevelPos[level - 1].transform.position,
                        LinePos     = this.View.LinePos
                    });
                    this.evolveItemUIs.Add(evolveItemUI);
                }
            }
            
            foreach (var evolveItemUI in this.evolveItemUIs)
            {
                evolveItemUI.Reorder();
            }
            return UniTask.CompletedTask;
        }

        public void Rebind()
        {
            foreach (var evolveItemUI in this.evolveItemUIs)
            {
                evolveItemUI.Dispose();
            }

            this.evolveItemUIs.Clear();
            this.BindData(this.Model);
        }
        
        public override void Dispose()
        {
            base.Dispose();
            this.evolveItemUIs.ForEach(item => item.Dispose());
            this.evolveItemUIs.Clear();
        }
        public override void CloseView()
        {
            this.View.viewField.transform.DOMove(this.View.startPos.position, 0.5f).SetEase(Ease.InOutQuint).onComplete += () =>
            {
                base.CloseView();
            };
        }
    }

    public class ElementEvolvePopupModel
    {
        public string ElementId;
    }
}