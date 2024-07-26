namespace Runtime.Scenes.Popups
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Zenject;

    public class LevelUpPopupModel
    {
        public LevelUpPopupModel( string inventoryId,Action onLevelUp)
        {
            this.OnLevelUp   = onLevelUp;
            this.InventoryId = inventoryId;
        }
        public Action OnLevelUp   { get; private set; }
        public string InventoryId { get; private set; }
    }

    public class LevelUpPopupView : BaseView
    {
    }

    [PopupInfo(nameof(LevelUpPopupView), isOverlay: true)]
    public class LevelUpPopupPresenter : BasePopupPresenter<LevelUpPopupView, LevelUpPopupModel>
    {
        private readonly InventoryLocalDataController inventoryLocalDataController;
        private readonly ItemBlueprint                itemBlueprint;
        public LevelUpPopupPresenter(SignalBus signalBus, ILogService logService, InventoryLocalDataController inventoryLocalDataController, ItemBlueprint itemBlueprint)
            : base(signalBus, logService)
        {
            this.inventoryLocalDataController = inventoryLocalDataController;
            this.itemBlueprint                = itemBlueprint;
        }
        public override UniTask BindData(LevelUpPopupModel popupModel)
        {
            return UniTask.CompletedTask;
        }
    }
}