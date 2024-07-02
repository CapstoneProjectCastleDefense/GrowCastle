namespace Runtime.Scenes.CharacterInventory
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Signals;
    using UnityEngine.UI;
    using Zenject;

    public class CharacterInventoryPopupView : BaseView
    {
        public CharacterInventoryAdapter characterInventoryAdapter;
        public Button                    exitBtn;
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
            this.SignalBus.Subscribe<RebindDataSignal>(this.RebindData);

            this.View.exitBtn.onClick.AddListener(this.CloseView);
        }

        private async void RebindData(RebindDataSignal signal)
        {
            if(signal.screenPresenterType != this.GetType()) return;
            await this.BindData();
        }

        public override async UniTask BindData()
        {
            var listModel = this.heroLocalDataController.GetAllHeroData().Select(e => new CharacterInventoryItemModel() { heroRuntimeData = e, resourceIcon = this.resourceBlueprint.GetDataById(e.resourceType).Image}).ToList();
            await this.View.characterInventoryAdapter.InitItemAdapter(listModel, this.diContainer);
        }
    }
}