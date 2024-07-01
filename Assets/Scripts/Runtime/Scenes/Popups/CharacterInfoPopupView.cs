namespace Runtime.Scenes.CharacterInventory
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Managers;
    using Spine.Unity;
    using TMPro;
    using UnityEngine.UI;
    using Zenject;

    public class CharacterInfoPopupModel
    {
        public SlotType        currentSelectedSlotType;
        public HeroRuntimeData heroRuntimeData;
    }

    public class CharacterInfoPopupView : BaseView
    {
        public SkeletonGraphic avatarAnim;
        public Button          equipBtn;
        public Button          buyBtn;
        public Button          unEquipBtn;
        public Button          levelUpBtn;
        public TextMeshProUGUI skillDescription;
        public TextMeshProUGUI attackInfo;
        public TextMeshProUGUI attackSpeedInfo;
        public Button          exitBtn;
    }

    [PopupInfo(nameof(CharacterInfoPopupView),isOverlay:true)]
    public class CharacterInfoPopupPresenter : BasePopupPresenter<CharacterInfoPopupView, CharacterInfoPopupModel>
    {
        private readonly IGameAssets             gameAssets;
        private readonly SkillBlueprint          skillBlueprint;
        private readonly SlotManager             slotManager;
        private readonly SlotLocalDataController slotLocalDataController;
        private readonly HeroLocalDataController heroLocalDataController;

        public CharacterInfoPopupPresenter(SignalBus signalBus, ILogService logService,IGameAssets gameAssets, SkillBlueprint skillBlueprint,SlotManager slotManager, SlotLocalDataController slotLocalDataController,HeroLocalDataController heroLocalDataController) : base(signalBus, logService)
        {
            this.gameAssets              = gameAssets;
            this.skillBlueprint          = skillBlueprint;
            this.slotManager             = slotManager;
            this.slotLocalDataController = slotLocalDataController;
            this.heroLocalDataController = heroLocalDataController;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.equipBtn.onClick.AddListener(this.OnEquipButtonClick);
            this.View.buyBtn.onClick.AddListener(this.OnUnlockButtonClick);
            this.View.unEquipBtn.onClick.AddListener(this.OnUnEquipButtonClick);
            this.View.exitBtn.onClick.AddListener(this.CloseView);
        }

        public override async UniTask BindData(CharacterInfoPopupModel popupModel)
        {
            var skeletonDataAsset = this.gameAssets.LoadAssetAsync<SkeletonDataAsset>(popupModel.heroRuntimeData.heroRecord.SkeletonDataAsset).WaitForCompletion();
            this.View.avatarAnim.ChangeSkeletonDataAsset(skeletonDataAsset,"idle");
            this.View.skillDescription.text = this.skillBlueprint.GetDataById(popupModel.heroRuntimeData.heroRecord.SkillToAnimationRecords.First().Key).Description;
            this.View.attackInfo.text       = $"{popupModel.heroRuntimeData.attack}";
            this.View.attackSpeedInfo.text  = $"{popupModel.heroRuntimeData.attackSpeed}";

            this.View.equipBtn.gameObject.SetActive(false);
            this.View.levelUpBtn.gameObject.SetActive(false);
            this.View.unEquipBtn.gameObject.SetActive(false);
            this.View.buyBtn.gameObject.SetActive(false);

            switch (popupModel.heroRuntimeData.heroStatus)
            {
                case HeroStatus.Lock:
                    this.View.buyBtn.gameObject.SetActive(true);

                    break;
                case HeroStatus.UnLock:
                    this.View.equipBtn.gameObject.SetActive(true);
                    this.View.levelUpBtn.gameObject.SetActive(true);

                    break;
                case HeroStatus.Equip:
                    this.View.unEquipBtn.gameObject.SetActive(true);
                    this.View.levelUpBtn.gameObject.SetActive(true);

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnEquipButtonClick()
        {
            this.slotManager.EquipHero(this.Model.heroRuntimeData.heroRecord.HeroId);
        }

        private void OnUnEquipButtonClick()
        {
            this.slotManager.UnEquipHero();
        }

        private async void OnUnlockButtonClick()
        {
            switch (this.Model.currentSelectedSlotType)
            {
                case SlotType.Hero:
                    var heroId = this.Model.heroRuntimeData.heroRecord.HeroId;
                    if (this.heroLocalDataController.UnLockHero(heroId))
                    {
                        this.Model.heroRuntimeData = this.heroLocalDataController.GetHeroRuntimeData(heroId);
                        await this.BindData(this.Model);
                    }
                    break;
            }
        }


    }
}