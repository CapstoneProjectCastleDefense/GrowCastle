namespace Runtime.Scenes.Popups
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Zenject;

    public class ItemDetailPopupModel
    {
    }

    public class ItemDetailPopupView : BaseView
    {
    }

    [PopupInfo(nameof(ItemDetailPopupView), isCloseWhenTapOutside: false)]
    public class ItemDetailPopupPresenter : BasePopupPresenter<ItemDetailPopupView, ItemDetailPopupModel>
    {
        public ItemDetailPopupPresenter(SignalBus signalBus, ILogService logService) : base(signalBus, logService) { }
        public override async UniTask BindData(ItemDetailPopupModel popupModel) { }
    }
}