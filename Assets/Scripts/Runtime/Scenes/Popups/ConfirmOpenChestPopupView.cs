namespace Runtime.Scenes.Popups
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Scenes.Adapters.Chest;
    using Runtime.Signals;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class ConfirmOpenChestPopupModel
    {
        public ResourceType ChestType;
    }

    public class ConfirmOpenChestPopupView : BaseView
    {
        public Image                  chestIcon;
        public Button                 cancelBtn;
        public Button                 openBtn;
        public TextMeshProUGUI        chestType;
        public ChestRewardItemAdapter chestRewardItemAdapter;
        public TextMeshProUGUI        totalItemCanGet;
    }

    [PopupInfo(nameof(ConfirmOpenChestPopupView), isOverlay: true)]
    public class ConfirmOpenChestPopupPresenter : BasePopupPresenter<ConfirmOpenChestPopupView, ConfirmOpenChestPopupModel>
    {
        private readonly ChestLocalDataController chestLocalDataController;
        private readonly IGameAssets              gameAssets;
        private readonly ScreenManager            screenManager;
        private readonly DiContainer              diContainer;
        private readonly ChestBlueprint           chestBlueprint;
        public ConfirmOpenChestPopupPresenter(SignalBus signalBus, ILogService logService, ChestLocalDataController chestLocalDataController, IGameAssets gameAssets, ScreenManager screenManager, DiContainer diContainer, ChestBlueprint chestBlueprint)
            : base(signalBus, logService)
        {
            this.chestLocalDataController = chestLocalDataController;
            this.gameAssets               = gameAssets;
            this.screenManager            = screenManager;
            this.diContainer              = diContainer;
            this.chestBlueprint           = chestBlueprint;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.cancelBtn.onClick.AddListener(this.OnCancelBtnClick);
            this.View.openBtn.onClick.AddListener(this.OnOpenBtnClick);
        }
        public override UniTask BindData(ConfirmOpenChestPopupModel popupModel)
        {
            var chestData = this.chestLocalDataController.GetChestData(popupModel.ChestType);
            this.View.chestIcon.sprite = this.gameAssets.LoadAssetAsync<Sprite>(chestData.ChestRecord.ChestIcon).WaitForCompletion();
            this.View.chestType.text   = this.chestBlueprint.GetDataById(chestData.ChestType).ChestName;
            var listItemModel = chestData.ChestRecord.PoolItems.Select(e=>new ChestRewardItemModel(){PoolItem = e}).ToList();
            this.View.chestRewardItemAdapter.InitItemAdapter(listItemModel, this.diContainer).Forget();
            this.View.totalItemCanGet.text = $"gacha {chestData.ChestRecord.ItemQuantity} item in this list item";
            return UniTask.CompletedTask;
        }

        private void OnCancelBtnClick() { this.CloseView(); }

        private async void OnOpenBtnClick()
        {
            await this.screenManager.OpenScreen<OpenChestPopupPresenter, OpenChestPopupModel>(
                new OpenChestPopupModel() { ListItemData = this.chestLocalDataController.OpenChest(this.Model.ChestType) });
            this.SignalBus.Fire<OpenChestSignal>();
            this.CloseView();
        }
    }
}