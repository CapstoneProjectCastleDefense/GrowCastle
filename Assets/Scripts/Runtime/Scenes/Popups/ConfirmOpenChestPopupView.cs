namespace Runtime.Scenes.Popups
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Signals;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class ConfirmOpenChestPopupModel
    {
        public ChestType ChestType;
    }

    public class ConfirmOpenChestPopupView : BaseView
    {
        public Image  chestIcon;
        public Button cancelBtn;
        public Button openBtn;
    }

    [PopupInfo(nameof(ConfirmOpenChestPopupView), isOverlay: true)]
    public class ConfirmOpenChestPopupPresenter : BasePopupPresenter<ConfirmOpenChestPopupView, ConfirmOpenChestPopupModel>
    {
        private readonly ChestLocalDataController chestLocalDataController;
        private readonly IGameAssets              gameAssets;
        private readonly ScreenManager            screenManager;
        public ConfirmOpenChestPopupPresenter(SignalBus signalBus, ILogService logService, ChestLocalDataController chestLocalDataController, IGameAssets gameAssets, ScreenManager screenManager)
            : base(signalBus, logService)
        {
            this.chestLocalDataController = chestLocalDataController;
            this.gameAssets               = gameAssets;
            this.screenManager            = screenManager;
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