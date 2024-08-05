namespace Runtime.Managers
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.UIModule.ScreenFlow.Managers;
    using GameFoundation.Scripts.Utilities.Extension;
    using global::Extensions;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Models.Tags;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Slot;
    using Runtime.Managers.Base;
    using Runtime.StateMachines.GameStateMachine;
    using Runtime.StateMachines.GameStateMachine.States;
    using Runtime.StaticValues;

    public class SlotManager : BaseElementManager<SlotModel, SlotPresenter, SlotView>
    {
        private readonly SlotLocalDataController slotLocalDataController;
        private readonly HeroManager             heroManager;
        private readonly LeaderManager           leaderManager;
        private readonly TowerManager            towerManager;
        private readonly HeroLocalDataController heroLocalDataController;
        private readonly EffectManager           effectManager;
        private readonly ScreenManager           screenManager;
        private readonly StatEffectBlueprint     statEffectBlueprint;
        private          SlotPresenter           currentSelectedSlot;
        private          GameStateMachine        gameStateMachine;

        public SlotManager(
            BaseElementPresenter<SlotModel, SlotView, SlotPresenter>.Factory factory,
            SlotLocalDataController slotLocalDataController,
            HeroManager heroManager,
            LeaderManager leaderManager,
            TowerManager towerManager,
            HeroLocalDataController heroLocalDataController,
            EffectManager effectManager,
            ScreenManager screenManager,
            StatEffectBlueprint statEffectBlueprint)
            : base(factory)
        {
            this.slotLocalDataController = slotLocalDataController;
            this.heroManager             = heroManager;
            this.leaderManager           = leaderManager;
            this.towerManager            = towerManager;
            this.heroLocalDataController = heroLocalDataController;
            this.effectManager           = effectManager;
            this.screenManager           = screenManager;
            this.statEffectBlueprint     = statEffectBlueprint;
        }

        public override void Initialize()
        {
            this.gameStateMachine = this.GetCurrentContainer().Resolve<GameStateMachine>();
        }

        [Obsolete("Obsolete")] public override void Tick()
        {
            base.Tick();
            if (this.gameStateMachine.CurrentState is GamePrepareState && this.screenManager.CurrentOverlayRoot.GetChildCount() == 0)
            {
                this.SetActiveRayCastAllSlot(true);
                return;
            }

            this.SetActiveRayCastAllSlot(false);
        }

        public SlotModel GetCurrentSelectedSlotModel() => this.currentSelectedSlot.Model;

        public void EquipHero(string heroId)
        {
            var currentSlotModel = this.GetCurrentSelectedSlotModel();
            var currentSlotData  = this.slotLocalDataController.GetSlotData(currentSlotModel.SlotRecord.Id);

            if (currentSlotData.DeployObjectId != null)
            {
                this.heroManager.entities.First(hero => hero.Model.Id.Equals(currentSlotData.DeployObjectId)).Dispose();
                this.heroLocalDataController.UnEquipHero(currentSlotData.DeployObjectId);
            }
            else
            {
                var slotHoldHero = this.slotLocalDataController.GetSlotHoldHero(heroId);
                if (slotHoldHero != null)
                {
                    this.slotLocalDataController.UnEquipCharacter(slotHoldHero.SlotId);
                    this.heroManager.entities.First(hero => hero.Model.Id.Equals(heroId)).Dispose();
                    this.heroLocalDataController.UnEquipHero(heroId);
                }
            }

            this.slotLocalDataController.EquipCharacter(this.GetCurrentSelectedSlotModel().SlotRecord.Id, heroId);
            this.heroLocalDataController.EquipHero(heroId);
            var heroRuntimeData = this.heroLocalDataController.GetHeroRuntimeData(heroId);
            switch (heroRuntimeData.heroRecord.HeroType)
            {
                case SlotType.Hero:
                    var hero = this.heroManager.CreateSingleHero(heroId, this.currentSelectedSlot.GetSlotView.heroPos);
                    if (!this.GetCurrentSelectedSlotModel().SlotRecord.EffectId.IsNullOrEmpty())
                    {
                        this.effectManager.AddEffectToTarget(hero, new ChangeStatTag(){EffectStatId = this.GetCurrentSelectedSlotModel().SlotRecord.EffectId});
                    }

                    break;
                case SlotType.Tower:
                    this.towerManager.CreateSingleTower(heroId, this.currentSelectedSlot.GetSlotView.heroPos);
                    break;
            }
        }

        public void UnEquipHero()
        {
            var currentSlotModel = this.GetCurrentSelectedSlotModel();
            var currentSlotData  = this.slotLocalDataController.GetSlotData(currentSlotModel.SlotRecord.Id);
            if (currentSlotData.DeployObjectId != null)
            {
                if (currentSlotModel.SlotRecord.SlotType == SlotType.Hero)
                {
                    this.heroManager.entities.First(hero => hero.Model.Id.Equals(currentSlotData.DeployObjectId)).Dispose();
                    this.heroLocalDataController.UnEquipHero(currentSlotData.DeployObjectId);
                }
                if (currentSlotModel.SlotRecord.SlotType == SlotType.Tower)
                {
                    this.towerManager.entities.First(tower => tower.Model.Id.Equals(currentSlotData.DeployObjectId)).Dispose();
                    this.heroLocalDataController.UnEquipHero(currentSlotData.DeployObjectId);
                }
            }

            this.slotLocalDataController.UnEquipCharacter(this.GetCurrentSelectedSlotModel().SlotRecord.Id);
        }

        public override SlotPresenter CreateElement(SlotModel model)
        {
            var presenter = base.CreateElement(model);
            presenter.slotManager = this;

            return presenter;
        }

        public void CreateAllSlot() { this.slotLocalDataController.GetAllSlotData.ForEach(this.CreateSingleSlot); }

        private void CreateSingleSlot(SlotData slotData) { this.CreateSingleSlotAsync(slotData).Forget(); }

        private async UniTask CreateSingleSlotAsync(SlotData slotData)
        {
            var slotPresenter = this.CreateElement(
                new() { AddressableName = "BaseSlot", Id = slotData.SlotId.ToString(), SlotRecord = this.slotLocalDataController.GetSlotDataRecord(slotData.SlotId) });
            await slotPresenter.UpdateView();

            if (slotData.DeployObjectId.IsNullOrEmpty()) return;
            if (slotData.SlotType == SlotType.Hero)
            {
                var hero = this.heroManager.CreateSingleHero(slotData.DeployObjectId, slotPresenter.GetSlotView.heroPos);
                if (!slotPresenter.Model.SlotRecord.EffectId.IsNullOrEmpty())
                {
                    this.effectManager.AddEffectToTarget(hero,new ChangeStatTag(){EffectStatId = slotPresenter.Model.SlotRecord.EffectId} );
                }
            }
            else if (slotData.SlotType == SlotType.Leader)
            {
                this.leaderManager.CreateSingleLeader(slotData.DeployObjectId, slotPresenter.GetSlotView.heroPos);
            }
            else if (slotData.SlotType == SlotType.Tower)
            {
                this.towerManager.CreateSingleTower(slotData.DeployObjectId, slotPresenter.GetSlotView.heroPos);
            }
        }

        public void UpdateCurrentSelectedSlot(SlotPresenter slotPresenter) { this.currentSelectedSlot = slotPresenter; }

        public void DeActiveAllSlot() => this.entities.ForEach(e => e.DeActiveView());
        public void ActiveAllSlot()   => this.entities.ForEach(e => e.ActiveView());

        public void SetActiveRayCastAllSlot(bool isActive) => this.entities.ForEach(e => e.SetActiveRayCast(isActive));

        public void UpdateAllSlots() { this.entities.ForEach(presenter => { presenter.UpdateSlotBaseOnCurrentLevel(); }); }
    }
}