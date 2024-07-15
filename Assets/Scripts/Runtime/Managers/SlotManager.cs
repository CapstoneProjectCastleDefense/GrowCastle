using TypeExtension = Extensions.TypeExtension;

namespace Runtime.Managers
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Slot;
    using Runtime.Managers.Base;

    public class SlotManager : BaseElementManager<SlotModel, SlotPresenter, SlotView>
    {
        private readonly SlotLocalDataController slotLocalDataController;
        private readonly HeroManager             heroManager;
        private readonly LeaderManager           leaderManager;
        private readonly TowerManager            towerManager;
        private readonly HeroLocalDataController heroLocalDataController;
        private          SlotPresenter           currentSelectedSlot;

        public SlotManager(
            BaseElementPresenter<SlotModel, SlotView, SlotPresenter>.Factory factory,
            SlotLocalDataController slotLocalDataController,
            HeroManager heroManager,
            LeaderManager leaderManager,
            TowerManager towerManager,
            HeroLocalDataController heroLocalDataController)
            : base(factory)
        {
            this.slotLocalDataController = slotLocalDataController;
            this.heroManager             = heroManager;
            this.leaderManager           = leaderManager;
            this.towerManager            = towerManager;
            this.heroLocalDataController = heroLocalDataController;
        }

        public override void Initialize() { }

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
                    this.heroManager.CreateSingleHero(heroId, this.currentSelectedSlot.GetSlotView.heroPos);
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

            if (TypeExtension.IsNullOrEmpty(slotData.DeployObjectId)) return;
            if (slotData.SlotType == SlotType.Hero)
            {
                this.heroManager.CreateSingleHero(slotData.DeployObjectId, slotPresenter.GetSlotView.heroPos);
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

        public void UpdateAllSlots() { this.entities.ForEach(presenter => { presenter.UpdateSlotBaseOnCurrentLevel(); }); }
    }
}