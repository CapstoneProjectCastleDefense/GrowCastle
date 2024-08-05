namespace Runtime.Scenes.Popups
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.Presenter;
    using GameFoundation.Scripts.UIModule.ScreenFlow.BaseScreen.View;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.LogService;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using Runtime.Scenes.Commons;
    using Runtime.Services;
    using TMPro;
    using UnityEngine;
    using UnityEngine.UI;
    using Zenject;

    public class CharacterInfoPopupModel
    {
        public SlotType        CurrentSelectedSlotType { get; set; }
        public HeroRuntimeData HeroRuntimeData         { get; set; }
        public IEquippable     Equippable              { get; private set; }
        public bool            IsInfoOnly              { get; set; }

        public CharacterInfoPopupModel(SlotType currentSelectedSlotType, HeroRuntimeData heroRuntimeData, IEquippable equippable, bool isInfoOnly = false)
        {
            this.CurrentSelectedSlotType = currentSelectedSlotType;
            this.HeroRuntimeData         = heroRuntimeData;
            this.Equippable              = equippable;
            this.IsInfoOnly              = isInfoOnly;
        }
    }

    public class CharacterInfoPopupView : BaseView
    {
        public Button              equipBtn;
        public Button              buyBtn;
        public Button              unEquipBtn;
        public Button              levelUpBtn;
        public TextMeshProUGUI     title;
        public Button              exitBtn;
        public List<EquipmentSlot> equipmentSlots;

        public TextMeshProUGUI bonusAttackSlot;
        public TextMeshProUGUI bonusAttackSpeedSlot;
        public TextMeshProUGUI bonusRecudeCooldownSlot;

        public Button changeClassBtn;

        public GameObject viewField;
        public Transform  startPos;
        public Transform  endPos;

        [SerializeField] private ElementGenericInfoView elementGenericInfoView;
        [SerializeField] private TMP_Text               levelUpCostTxt;

        public ElementGenericInfoView ElementGenericInfoView => this.elementGenericInfoView;
        public TMP_Text               LevelUpCostTxt         => this.levelUpCostTxt;
    }

    [PopupInfo(nameof(CharacterInfoPopupView), isOverlay: true)]
    public class CharacterInfoPopupPresenter : BasePopupPresenter<CharacterInfoPopupView, CharacterInfoPopupModel>
    {
        private readonly SlotManager                         slotManager;
        private readonly HeroLocalDataController             heroLocalDataController;
        private readonly DiContainer                         diContainer;
        private readonly ScreenManager                       screenManager;
        private readonly ElementEvolutionLocalDataController elementEvolutionLocalDataController;
        private readonly HeroUpgradeService                  heroUpgradeService;
        private readonly SlotBlueprint                       slotBlueprint;
        private readonly StatEffectBlueprint                 statEffectBlueprint;

        public CharacterInfoPopupPresenter(
            SignalBus signalBus,
            ILogService logService,
            SlotManager slotManager,
            HeroLocalDataController heroLocalDataController,
            DiContainer diContainer,
            ScreenManager screenManager,
            ElementEvolutionLocalDataController elementEvolutionLocalDataController,
            HeroUpgradeService heroUpgradeService,
            SlotBlueprint slotBlueprint,
            StatEffectBlueprint statEffectBlueprint
        )
            : base(signalBus, logService)
        {
            this.slotManager                         = slotManager;
            this.heroLocalDataController             = heroLocalDataController;
            this.diContainer                         = diContainer;
            this.screenManager                       = screenManager;
            this.elementEvolutionLocalDataController = elementEvolutionLocalDataController;
            this.heroUpgradeService                  = heroUpgradeService;
            this.slotBlueprint                       = slotBlueprint;
            this.statEffectBlueprint                 = statEffectBlueprint;
        }

        protected override void OnViewReady()
        {
            base.OnViewReady();
            this.View.equipBtn.onClick.AddListener(this.OnEquipButtonClick);
            this.View.buyBtn.onClick.AddListener(this.OnUnlockButtonClick);
            this.View.unEquipBtn.onClick.AddListener(this.OnUnEquipButtonClick);
            this.View.exitBtn.onClick.AddListener(this.CloseView);
            this.View.changeClassBtn.onClick.AddListener(this.ChangeClass);
            this.View.levelUpBtn.onClick.AddListener(this.LevelUp);
            foreach (var viewEquipmentSlot in this.View.equipmentSlots)
            {
                this.diContainer.Inject(viewEquipmentSlot);
            }

            this.diContainer.InjectGameObject(this.View.ElementGenericInfoView.gameObject);
        }

        public override async UniTask BindData(CharacterInfoPopupModel popupModel)
        {
            this.View.changeClassBtn.gameObject.SetActive(!popupModel.IsInfoOnly);

            this.View.title.text                   = this.slotBlueprint.GetDataById(int.Parse(this.slotManager.GetCurrentSelectedSlotModel().Id)).SlotType.ToString();
            this.View.viewField.transform.position = this.View.startPos.position;
            this.View.viewField.transform.DOMove(this.View.endPos.position, 0.5f).SetEase(Ease.InOutQuint);
            var equipmentList = this.heroLocalDataController.GetEquipments(this.Model.HeroRuntimeData.heroRecord.HeroId);
            for (var i = 0; i < this.View.equipmentSlots.Count; i++)
            {
                await this.View.equipmentSlots[i].BindData(new(this.Model.Equippable, equipmentList.Count > i ? equipmentList[i] : "", this.ReBindData));
            }

            this.View.LevelUpCostTxt.text = $"{this.heroUpgradeService.GetUpgradeCost(this.Model.HeroRuntimeData.heroRecord.HeroId)}";
            var effectSlotRecord = this.statEffectBlueprint.GetDataById(this.slotBlueprint.GetDataById(int.Parse(this.slotManager.GetCurrentSelectedSlotModel().Id)).EffectId);
            this.View.bonusAttackSlot.text         = $"+{effectSlotRecord.AttackBonusPercent}%";
            this.View.bonusAttackSpeedSlot.text    = $"+{effectSlotRecord.AttackSpeedBonusPercent}%";
            this.View.bonusRecudeCooldownSlot.text = $"-{effectSlotRecord.SkillCooldownBonusPercent}%";

            this.BindGenericInfo(popupModel);
            this.UpdateView(popupModel);
        }

        private void UpdateView(CharacterInfoPopupModel popupModel)
        {
            this.Model = popupModel;

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

        private void BindGenericInfo(CharacterInfoPopupModel model)
        {
            var id          = model.HeroRuntimeData.heroRecord.HeroId;
            var evolutionId = this.elementEvolutionLocalDataController.GetEvolutionElementData(id).EvolutionId;
            this.View.ElementGenericInfoView.BindData(new ElementGenericInfoModel()
            {
                ElementId   = model.HeroRuntimeData.heroRecord.HeroId,
                EvolutionId = evolutionId
            });
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
            switch (this.slotBlueprint.GetDataById(int.Parse(this.slotManager.GetCurrentSelectedSlotModel().Id)).SlotType)
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

        private async void ReBindData()
        {
            var heroId = this.Model.HeroRuntimeData.heroRecord.HeroId;
            this.Model.HeroRuntimeData = this.heroLocalDataController.GetHeroRuntimeData(heroId);
            var equipmentList = this.heroLocalDataController.GetEquipments(this.Model.HeroRuntimeData.heroRecord.HeroId);
            for (var i = 0; i < this.View.equipmentSlots.Count; i++)
            {
                await this.View.equipmentSlots[i].BindData(new(this.Model.Equippable, equipmentList.Count > i ? equipmentList[i] : "", this.ReBindData));
            }

            this.UpdateView(this.Model);

            this.View.ElementGenericInfoView.Rebind();
        }

        private void ChangeClass()
        {
            base.CloseView();
            this.screenManager.OpenScreen<ElementEvolvePopupPresenter, ElementEvolvePopupModel>(new ElementEvolvePopupModel()
                {
                    CharacterId = this.Model.HeroRuntimeData.heroRecord.HeroId
                })
                .Forget();
        }

        private void LevelUp()
        {
            this.heroLocalDataController.UpgradeHero(this.Model.HeroRuntimeData.heroRecord.HeroId);
            this.View.LevelUpCostTxt.text = $"{this.heroUpgradeService.GetUpgradeCost(this.Model.HeroRuntimeData.heroRecord.HeroId)}";
            this.ReBindData();
        }

        public override void CloseView() { this.View.viewField.transform.DOMove(this.View.startPos.position, 0.5f).SetEase(Ease.Linear).onComplete += () => { base.CloseView(); }; }
    }
}