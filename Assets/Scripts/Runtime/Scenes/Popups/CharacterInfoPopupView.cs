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
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using Runtime.Scenes.Commons;
    using TMPro;
    using UnityEngine;
    using UnityEngine.Serialization;
    using UnityEngine.UI;
    using Zenject;

    public class CharacterInfoPopupModel
    {
        public SlotType        CurrentSelectedSlotType { get; private set; }
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


        public Button changeClassBtn;

        public GameObject viewField;
        public Transform  startPos;
        public Transform  endPos;

        [FormerlySerializedAs("characterGenericInfoView")] [SerializeField]
        private ElementGenericInfoView elementGenericInfoView;

        public ElementGenericInfoView ElementGenericInfoView => this.elementGenericInfoView;
    }

    [PopupInfo(nameof(CharacterInfoPopupView), isOverlay: true)]
    public class CharacterInfoPopupPresenter : BasePopupPresenter<CharacterInfoPopupView, CharacterInfoPopupModel>
    {
        private readonly SlotManager                       slotManager;
        private readonly HeroLocalDataController           heroLocalDataController;
        private readonly DiContainer                       diContainer;
        private readonly ScreenManager                     screenManager;
        private readonly ElementLocalDataController        elementLocalDataController;
        private readonly ElementUpgradeLocalDataController elementUpgradeLocalDataController;

        public CharacterInfoPopupPresenter(SignalBus signalBus,
            ILogService logService,
            SlotManager slotManager,
            HeroLocalDataController heroLocalDataController,
            DiContainer diContainer,
            ScreenManager screenManager,
            ElementLocalDataController elementLocalDataController,
            ElementUpgradeLocalDataController elementUpgradeLocalDataController)
            : base(signalBus, logService)
        {
            this.slotManager                       = slotManager;
            this.heroLocalDataController           = heroLocalDataController;
            this.diContainer                       = diContainer;
            this.screenManager                     = screenManager;
            this.elementLocalDataController        = elementLocalDataController;
            this.elementUpgradeLocalDataController = elementUpgradeLocalDataController;
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

            this.View.title.text                   = this.Model.CurrentSelectedSlotType.ToString();
            this.View.viewField.transform.position = this.View.startPos.position;
            this.View.viewField.transform.DOMove(this.View.endPos.position, 0.5f).SetEase(Ease.InOutQuint);
            var equipmentList = this.heroLocalDataController.GetEquipments(this.Model.HeroRuntimeData.heroRecord.HeroId);
            for (var i = 0; i < this.View.equipmentSlots.Count; i++)
            {
                await this.View.equipmentSlots[i].BindData(new(this.Model.Equippable, equipmentList.Count > i ? equipmentList[i] : "", this.ReBindData));
            }

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
            var evolutionId = this.elementLocalDataController.GetEvolutionElementData(id).EvolutionId;
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
            this.elementUpgradeLocalDataController.UpgradeElement(this.Model.HeroRuntimeData.heroRecord.HeroId);   
        }

        public override void CloseView()
        {
            this.View.viewField.transform.DOMove(this.View.startPos.position, 0.5f).SetEase(Ease.Linear).onComplete += () => { base.CloseView(); };
        }
    }
}