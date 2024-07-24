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
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class CharacterEvolvePopupView : BaseView
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

    [PopupInfo(nameof(CharacterEvolvePopupView), isOverlay: true)]
    public class CharacterEvolvePopupPresenter : BasePopupPresenter<CharacterEvolvePopupView, CharacterEvolvePopupModel>
    {
        private readonly EvolutionBlueprint evolutionBlueprint;
        private readonly ObjectPoolManager  objectPoolManager;

        public CharacterEvolvePopupPresenter(SignalBus signalBus,
                                             ILogService logService,
                                             EvolutionBlueprint evolutionBlueprint,
                                             ObjectPoolManager objectPoolManager)
            : base(signalBus, logService)
        {
            this.evolutionBlueprint = evolutionBlueprint;
            this.objectPoolManager  = objectPoolManager;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.CloseButton.onClick.AddListener(this.CloseView);
        }

        public override UniTask BindData(CharacterEvolvePopupModel popupModel)
        {
            var evolutionRecord = this.evolutionBlueprint[popupModel.CharacterId];
            foreach (var (level, record) in evolutionRecord.LevelToEvolutionDetailRecords)
            {
                foreach (var (_, evolutionDetailRecord) in record.EvolutionDetailRecords)
                {
                    var evolveItemUI = this.objectPoolManager.Spawn(this.View.EvolveItemUI, this.View.EvolveItemUIContainer.transform);
                    evolveItemUI.BindData(new EvolveItemUIModel
                    {
                        EvolutionDetailRecord = evolutionDetailRecord,
                        LevelPos              = this.View.LevelPos[level - 1].transform.position,
                        LinePos               = this.View.LinePos
                    });
                }
            }

            return UniTask.CompletedTask;
        }
    }

    public class CharacterEvolvePopupModel
    {
        public string CharacterId;
    }
}