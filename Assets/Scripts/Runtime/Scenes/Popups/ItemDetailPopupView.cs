namespace Runtime.Scenes.Popups
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class ItemDetailPopupModel
    {
        public ItemDetailPopupModel(IItemModel itemModel, IEquippable equippable)
        {
            this.ItemModel  = itemModel;
            this.Equippable = equippable;
        }
        public IItemModel  ItemModel  { get; set; }
        public IEquippable Equippable { get; set; }
    }

    public class ItemDetailPopupView : BaseView
    {
        public Button CloseButton;
        public Image  ItemImage;
        public Image  RarityImage;
    }

    [PopupInfo(nameof(ItemDetailPopupView), isCloseWhenTapOutside: false)]
    public class ItemDetailPopupPresenter : BasePopupPresenter<ItemDetailPopupView, ItemDetailPopupModel>
    {
        private readonly IGameAssets gameAssets;
        public ItemDetailPopupPresenter(SignalBus signalBus, ILogService logService, IGameAssets gameAssets) : base(signalBus, logService)
        {
            this.gameAssets = gameAssets;
        }
        public override async UniTask BindData(ItemDetailPopupModel popupModel)
        {
            this.View.CloseButton.onClick.RemoveAllListeners();
            this.View.CloseButton.onClick.AddListener(this.CloseView);
            this.View.RarityImage.sprite = await this.gameAssets.LoadAssetAsync<Sprite>("");
            this.View.ItemImage.sprite   = await this.gameAssets.LoadAssetAsync<Sprite>("");
        }
    }
}