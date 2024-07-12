namespace Runtime.Scenes.CharacterInventory
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class CharacterInventoryPopupView : BaseView
    {
        public CharacterInventoryAdapter characterInventoryAdapter;
        public Button                    exitBtn;
        public GameObject                viewField;
        public Transform                 startPos;
        public Transform                 endPos;
    }

    [PopupInfo(nameof(CharacterInventoryPopupView), isOverlay: true)]
    public class CharacterInventoryPopupPresenter : BasePopupPresenter<CharacterInventoryPopupView>
    {
        private readonly HeroLocalDataController heroLocalDataController;
        private readonly ResourceBlueprint       resourceBlueprint;
        private readonly DiContainer             diContainer;

        public CharacterInventoryPopupPresenter(SignalBus signalBus, HeroLocalDataController heroLocalDataController,ResourceBlueprint resourceBlueprint, DiContainer diContainer) : base(signalBus)
        {
            this.heroLocalDataController = heroLocalDataController;
            this.resourceBlueprint       = resourceBlueprint;
            this.diContainer             = diContainer;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.exitBtn.onClick.AddListener(this.CloseView);
        }
        public override async UniTask BindData()
        {
            this.View.viewField.transform.position = this.View.startPos.position;
            this.View.viewField.transform.DOMove(this.View.endPos.position, 0.5f).SetEase(Ease.InOutQuint);
            var listModel = this.heroLocalDataController.GetAllHeroRuntimeData().Select(e => new CharacterInventoryItemModel() { heroRuntimeData = e, resourceIcon = this.resourceBlueprint.GetDataById(e.resourceType).Image}).ToList();
            await this.View.characterInventoryAdapter.InitItemAdapter(listModel, this.diContainer);
        }
        public override void CloseView()
        {
            this.View.viewField.transform.DOMove(this.View.startPos.position, 0.5f).SetEase(Ease.InOutQuint).onComplete += () =>
            {
                base.CloseView();
            };
        }
    }
}