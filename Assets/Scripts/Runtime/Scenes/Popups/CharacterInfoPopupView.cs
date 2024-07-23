namespace Runtime.Scenes.Popups
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.Utilities.LogService;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using Spine.Unity;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class CharacterInfoPopupModel
    {
        public SlotType        CurrentSelectedSlotType { get; private set; }
        public HeroRuntimeData HeroRuntimeData         { get; set; }
        public IEquippable     Equippable              { get; private set; }
        public CharacterInfoPopupModel(SlotType currentSelectedSlotType, HeroRuntimeData heroRuntimeData, IEquippable equippable)
        {
            this.CurrentSelectedSlotType = currentSelectedSlotType;
            this.HeroRuntimeData         = heroRuntimeData;
            this.Equippable              = equippable;
        }
    }

    public class CharacterInfoPopupView : BaseView
    {
        public SkeletonGraphic     avatarAnim;
        public Button              equipBtn;
        public Button              buyBtn;
        public Button              unEquipBtn;
        public Button              levelUpBtn;
        public TextMeshProUGUI     skillDescription;
        public TextMeshProUGUI     attackInfo;
        public TextMeshProUGUI     attackSpeedInfo;
        public Button              exitBtn;
        public List<EquipmentSlot> equipmentSlots;

        public GameObject viewField;
        public Transform  startPos;
        public Transform  endPos;
    }

    [PopupInfo(nameof(CharacterInfoPopupView), isOverlay: true)]
    public class CharacterInfoPopupPresenter : BasePopupPresenter<CharacterInfoPopupView, CharacterInfoPopupModel>
    {
        private readonly IGameAssets             gameAssets;
        private readonly SkillBlueprint          skillBlueprint;
        private readonly SlotManager             slotManager;
        private readonly HeroLocalDataController heroLocalDataController;
        private readonly DiContainer             diContainer;

        public CharacterInfoPopupPresenter(SignalBus signalBus, ILogService logService, IGameAssets gameAssets, SkillBlueprint skillBlueprint, SlotManager slotManager,
            HeroLocalDataController heroLocalDataController, DiContainer diContainer) : base(signalBus, logService)
        {
            this.gameAssets              = gameAssets;
            this.skillBlueprint          = skillBlueprint;
            this.slotManager             = slotManager;
            this.heroLocalDataController = heroLocalDataController;
            this.diContainer             = diContainer;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.equipBtn.onClick.AddListener(this.OnEquipButtonClick);
            this.View.buyBtn.onClick.AddListener(this.OnUnlockButtonClick);
            this.View.unEquipBtn.onClick.AddListener(this.OnUnEquipButtonClick);
            this.View.exitBtn.onClick.AddListener(this.CloseView);
            foreach (var viewEquipmentSlot in this.View.equipmentSlots)
            {
                this.diContainer.Inject(viewEquipmentSlot);
            }
        }

        public override async UniTask BindData(CharacterInfoPopupModel popupModel)
        {
            this.View.viewField.transform.position = this.View.startPos.position;
            this.View.viewField.transform.DOMove(this.View.endPos.position, 0.5f).SetEase(Ease.InOutQuint);
            var equipmentList = this.heroLocalDataController.GetEquipments(this.Model.HeroRuntimeData.heroRecord.HeroId);
            for (var i = 0; i < this.View.equipmentSlots.Count; i++)
            {
                await this.View.equipmentSlots[i].BindData(new(this.Model.Equippable, equipmentList.Count > i ? equipmentList[i] : ""));
            }

            this.UpdateView(popupModel);
        }

        private void UpdateView(CharacterInfoPopupModel popupModel)
        {
            this.Model = popupModel;
            var skeletonDataAsset = this.gameAssets.LoadAssetAsync<SkeletonDataAsset>(popupModel.HeroRuntimeData.heroRecord.SkeletonDataAsset).WaitForCompletion();
            this.View.avatarAnim.ChangeSkeletonDataAsset(skeletonDataAsset, "idle");
            this.View.skillDescription.text = this.skillBlueprint.GetDataById(popupModel.HeroRuntimeData.heroRecord.ActiveSkill.skillName).Description;
            this.View.attackInfo.text       = $"{popupModel.HeroRuntimeData.attack}";
            this.View.attackSpeedInfo.text  = $"{popupModel.HeroRuntimeData.attackSpeed}";

            this.View.equipBtn.gameObject.SetActive(false);
            this.View.levelUpBtn.gameObject.SetActive(false);
            this.View.unEquipBtn.gameObject.SetActive(false);
            this.View.buyBtn.gameObject.SetActive(false);

            switch (popupModel.HeroRuntimeData.heroStatus)
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
            this.slotManager.EquipHero(this.Model.HeroRuntimeData.heroRecord.HeroId);
            this.ReBindData();
        }

        private void OnUnEquipButtonClick()
        {
            this.slotManager.UnEquipHero();
            this.ReBindData();
        }

        private void OnUnlockButtonClick()
        {
            switch (this.Model.CurrentSelectedSlotType)
            {
                case SlotType.Hero:
                    var heroId = this.Model.HeroRuntimeData.heroRecord.HeroId;
                    if (this.heroLocalDataController.UnLockHero(heroId))
                    {
                        this.ReBindData();
                    }

                    break;
                case SlotType.Tower:
                    break;
                case SlotType.Leader:
                    break;
                case SlotType.Archer:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ReBindData()
        {
            var heroId = this.Model.HeroRuntimeData.heroRecord.HeroId;
            this.Model.HeroRuntimeData = this.heroLocalDataController.GetHeroRuntimeData(heroId);
            this.UpdateView(this.Model);
        }

        public override void CloseView()
        {
            this.View.viewField.transform.DOMove(this.View.startPos.position, 0.5f).SetEase(Ease.OutElastic).onComplete += () => { base.CloseView(); };
        }
    }
}