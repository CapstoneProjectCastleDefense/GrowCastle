namespace Runtime.Scenes.Popups
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Signals;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class ChestPopupView : BaseView
    {
        public List<ChestView> chestViews;
        public Button          exitBtn;
    }

    [PopupInfo(nameof(ChestPopupView))] public class ChestPopupPresenter : BasePopupPresenter<ChestPopupView>
    {
        private readonly ChestLocalDataController chestLocalDataController;
        private readonly IGameAssets              gameAssets;
        private readonly ScreenManager            screenManager;
        public ChestPopupPresenter(SignalBus signalBus, ChestLocalDataController chestLocalDataController, IGameAssets gameAssets, ScreenManager screenManager)
            : base(signalBus)
        {
            this.chestLocalDataController = chestLocalDataController;
            this.gameAssets               = gameAssets;
            this.screenManager            = screenManager;
        }
        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.exitBtn.onClick.AddListener(this.CloseView);
            this.SignalBus.Subscribe<OpenChestSignal>(this.OnChestLocalDataUpdate);
        }
        public override UniTask BindData()
        {
            var allChestData = this.chestLocalDataController.GetAllChestLocalData();
            this.View.chestViews.ForEach(chestView =>
            {
                var data = allChestData.Where(e => e.ChestType == chestView.chestType).ToList();
                if (data.Count != 0)
                {
                    this.BindDataToChest(data, chestView);
                }
                else
                {
                    chestView.gameObject.SetActive(false);
                }
            });
            return UniTask.CompletedTask;
        }

        private void OnChestLocalDataUpdate(OpenChestSignal signal)
        {
            this.BindData();
        }

        private void BindDataToChest(List<ChestData> chestData, ChestView chestView)
        {
            if (chestData == null || chestData.Count == 0)
            {
                chestView.gameObject.SetActive(false);
                return;
            }

            var chestDataSample = chestData.First();
            chestView.chestIcon.sprite = this.gameAssets.LoadAssetAsync<Sprite>(chestDataSample.ChestRecord.ChestIcon).WaitForCompletion();
            chestView.chestNumber.text = $"{chestData.Count}";
            chestView.chestButton.onClick.RemoveAllListeners();
            chestView.chestButton.onClick.AddListener(() => { this.OpenChest(chestDataSample.ChestType); });
        }

        private async void OpenChest(ChestType chestType)
        {
            await this.screenManager.OpenScreen<ConfirmOpenChestPopupPresenter, ConfirmOpenChestPopupModel>(new ConfirmOpenChestPopupModel() { ChestType = chestType });
        }
    }
}