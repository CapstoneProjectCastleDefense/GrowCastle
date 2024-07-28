namespace Runtime.Scenes.Popups
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Zenject;

    public class TierUpPopupModel
    {
        public TierUpPopupModel(string inventoryId, Action onTierUp)
        {
            this.InventoryId = inventoryId;
            this.OnTierUp    = onTierUp;
        }
        public string InventoryId { get; private set; }
        public Action OnTierUp    { get; private set; }
    }

    public class TierUpPopupView : BaseView
    {
    }

    [PopupInfo(nameof(TierUpPopupView), isOverlay: true)]
    public class TierUpPopupPresenter : BasePopupPresenter<TierUpPopupView, TierUpPopupModel>
    {
        public TierUpPopupPresenter(SignalBus signalBus, ILogService logService) : base(signalBus, logService) { }
        public override UniTask BindData(TierUpPopupModel popupModel) { return UniTask.CompletedTask; }
    }
}