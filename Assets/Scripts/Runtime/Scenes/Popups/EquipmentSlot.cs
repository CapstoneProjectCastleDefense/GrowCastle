namespace Runtime.Scenes.Popups
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using global::Extensions;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class EquipmentSlotModel
    {
        public EquipmentSlotModel(IEquippable equippable, string inventoryId)
        {
            this.Equippable  = equippable;
            this.InventoryId = inventoryId;
        }
        public IEquippable Equippable  { get; set; }
        public string      InventoryId { get; set; }
    }

    public class EquipmentSlot : MonoBehaviour
    {
        private                  EquipmentSlotModel model;
        [SerializeField] private Button             button;
        [SerializeField] private Image              itemImage;
        [SerializeField] private Image              rarityImage;

        [Inject] private IGameAssets                  gameAssets;
        [Inject] private InventoryLocalDataController inventoryLocalDataController;
        [Inject] private IScreenManager               screenManager;

        public async UniTask BindData(EquipmentSlotModel model)
        {
            this.model = model;
            this.button.onClick.AddListener(this.OnClick);
            var isEmpty = this.model.InventoryId.IsNullOrEmpty();
            this.itemImage.gameObject.SetActive(!isEmpty);
            if (isEmpty)
            {
                this.rarityImage.sprite = await this.gameAssets.LoadAssetAsync<Sprite>(RarityEnum.Common.ToString());
                return;
            }

            var item = this.inventoryLocalDataController.GetItem(this.model.InventoryId);

            this.itemImage.sprite   = await this.gameAssets.LoadAssetAsync<Sprite>(item.AddressableName);
            this.rarityImage.sprite = await this.gameAssets.LoadAssetAsync<Sprite>(item.Rarity.ToString());
        }
        private void OnClick()
        {
            this.screenManager.OpenScreen<ItemInventoryPopupPresenter, ItemInventoryPopupModel>(new(this.model.Equippable, this.model.InventoryId)).Forget();
        }
    }
}