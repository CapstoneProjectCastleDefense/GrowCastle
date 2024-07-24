namespace Runtime.Scenes.Popups
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Models.Blueprints;
    using Runtime.Scenes.Adapters.Chest;
    using UnityEngine.UI;
    using Zenject;

    public class OpenChestPopupModel
    {
        public List<PoolItem> ListItemData;
    }

    public class OpenChestPopupView : BaseView
    {
        public ChestRewardItemAdapter chestRewardItemAdapter;
        public Button                 exitBtn;
    }

    [PopupInfo(nameof(OpenChestPopupView), isOverlay: true)]
    public class OpenChestPopupPresenter : BasePopupPresenter<OpenChestPopupView, OpenChestPopupModel>
    {
        private readonly DiContainer diContainer;
        public OpenChestPopupPresenter(SignalBus signalBus, ILogService logService, DiContainer diContainer)
            : base(signalBus, logService)
        {
            this.diContainer = diContainer;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.exitBtn.onClick.AddListener(this.CloseView);
        }
        public override async UniTask BindData(OpenChestPopupModel popupModel)
        {
            await this.View.chestRewardItemAdapter.InitItemAdapter(popupModel.ListItemData.Select(e => new ChestRewardItemModel()
            {
                PoolItem = e
            }).ToList(), this.diContainer);
        }
    }
}