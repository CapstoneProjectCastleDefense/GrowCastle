namespace Runtime.Scenes.Popups
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using Zenject;

    public class ItemInventoryPopupView : BaseView
    {
    }

    [PopupInfo(nameof(ItemInventoryPopupView), isCloseWhenTapOutside: false)]
    public class ItemInventoryPopupPresenter : BasePopupPresenter<ItemInventoryPopupView>
    {
        public ItemInventoryPopupPresenter(SignalBus signalBus) : base(signalBus) { }
        public override UniTask BindData() { return UniTask.CompletedTask; }
    }
}