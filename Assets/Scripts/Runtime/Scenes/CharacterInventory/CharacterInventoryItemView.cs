namespace Runtime.Scenes.CharacterInventory
{
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.MVP;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using Models.LocalData.LocalDataController;
    using Spine.Unity;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;

    public class CharacterInventoryItemModel
    {
        public HeroRuntimeData heroRuntimeData;
        public string          resourceIcon;
    }

    public class CharacterInventoryItemView : TViewMono
    {
        public SkeletonGraphic characterAnim;
        public Image           resourceIcon;
        public GameObject      resourceField;
        public TextMeshProUGUI resourceValueText;
        public Button          selectBtn;
    }

    public class CharacterInventoryItemPresenter : BaseUIItemPresenter<CharacterInventoryItemView, CharacterInventoryItemModel>
    {
        private readonly HeroLocalDataController     heroLocalDataController;
        private readonly ScreenManager               screenManager;
        private          CharacterInventoryItemModel model;
        public CharacterInventoryItemPresenter(IGameAssets gameAssets, HeroLocalDataController heroLocalDataController, ScreenManager screenManager) : base(gameAssets)
        {
            this.heroLocalDataController = heroLocalDataController;
            this.screenManager           = screenManager;
        }

        public override void BindData(CharacterInventoryItemModel param)
        {
            this.model                       = param;
            this.View.characterAnim.ChangeSkeletonDataAsset(this.GameAssets.LoadAssetAsync<SkeletonDataAsset>(param.heroRuntimeData.heroRecord.SkeletonDataAsset).WaitForCompletion());
            this.View.resourceValueText.text = $"{param.heroRuntimeData.resourceValue}";
            this.View.resourceIcon.sprite    = this.GameAssets.LoadAssetAsync<Sprite>(param.resourceIcon).WaitForCompletion();
            this.View.resourceField.SetActive(param.heroRuntimeData.heroStatus == HeroStatus.Lock);

            this.View.selectBtn.onClick.RemoveAllListeners();
            this.View.selectBtn.onClick.AddListener(this.OnSelectButtonClick);
        }

        private async void OnSelectButtonClick()
        {
            var characterInfoModel = new CharacterInfoPopupModel()
            {
                heroRuntimeData = this.model.heroRuntimeData,
            };

            //try set hero status to unlock because can use hero in other slot and reset in current slot
            if (characterInfoModel.heroRuntimeData.heroStatus == HeroStatus.Equip) characterInfoModel.heroRuntimeData.heroStatus = HeroStatus.UnLock;
            await this.screenManager.OpenScreen<CharacterInfoPopupPresenter, CharacterInfoPopupModel>(characterInfoModel);
        }
    }
}