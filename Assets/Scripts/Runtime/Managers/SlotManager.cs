using TypeExtension = Extensions.TypeExtension;

namespace Runtime.Managers
{
    using Cysharp.Threading.Tasks;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Slot;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers.Base;
    using System;

    public class SlotManager : BaseElementManager<SlotModel, SlotPresenter, SlotView>
    {
        private readonly SlotLocalDataController slotLocalDataController;
        private readonly HeroManager             heroManager;
        private          SlotPresenter           currentSelectedSlot;

        public SlotManager(BaseElementPresenter<SlotModel, SlotView, SlotPresenter>.Factory factory, SlotLocalDataController slotLocalDataController, HeroManager heroManager)
            : base(factory)
        {
            this.slotLocalDataController = slotLocalDataController;
            this.heroManager             = heroManager;
        }

        public override void Initialize()
        {
        }

        public override void DisposeAllElement()
        {
        }

        public void EquipHeo(IHeroPresenter heroPresenter)
        {
            this.currentSelectedSlot.LoadHero(heroPresenter);
        }

        public override SlotPresenter CreateElement(SlotModel model)
        {
            var presenter = base.CreateElement(model);
            presenter.slotManager = this;

            return presenter;
        }

        public void CreateAllSlot()
        {
            this.slotLocalDataController.GetAllSlotData.ForEach(this.CreateSingleSlot);
        }

        private void CreateSingleSlot(SlotData slotData)
        {
            this.CreateSingleSlotAsync(slotData).Forget();
        }

        private async UniTask CreateSingleSlotAsync(SlotData slotData)
        {
            var slotPresenter = this.CreateElement(new() { AddressableName = "BaseSlot", Id = slotData.SlotId.ToString(), SlotRecord = this.slotLocalDataController.GetSlotDataRecord(slotData.SlotId) });
            await slotPresenter.UpdateView();

            if (TypeExtension.IsNullOrEmpty(slotData.DeployObjectId)) return;
            if (slotData.SlotType == SlotType.Hero)
            {
                this.heroManager.CreateSingleHero(slotData.DeployObjectId, slotPresenter.GetSlotView.heroPos);
            }
        }

        public void UpdateCurrentSelectedSlot(SlotPresenter slotPresenter)
        {
            this.currentSelectedSlot = slotPresenter;
        }

        public void DeActiveAllSlot() => this.entities.ForEach(e => e.DeActiveView());

        public void UpdateSlotBaseOnCurrentLevel() {
            this.entities.ForEach(presenter =>
            {
                presenter.UpdateSlotBaseOnCurrentLevel();
            });
        }
    }
}