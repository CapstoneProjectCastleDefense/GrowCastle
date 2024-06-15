namespace Runtime.Managers
{
    using Cysharp.Threading.Tasks;
    using Extensions;
    using global::Extensions;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Elements.Entities.Slot;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers.Base;

    public class SlotManager : BaseElementManager<SlotModel,SlotPresenter,SlotView>
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
            this.slotLocalDataController.GetAllSlotData.ForEach( this.CreateSingleSlot);
        }

        private async void CreateSingleSlot(SlotData slotData)
        {
            var slotPresenter = this.CreateElement(new() { AddressableName = "BaseSlot", Id = slotData.SlotId.ToString(), SlotRecord = this.slotLocalDataController.GetSlotDataRecord(slotData.SlotId) });
            await slotPresenter.UpdateView();

            if (slotData.DeployObjectId.IsNullOrEmpty()) return;
            if (slotData.SlotType == SlotType.Hero)
            {
                this.heroManager.CreateElement(new() { Id = slotData.DeployObjectId, ParentView = slotPresenter.GetSlotView.heroPos }).UpdateView().Forget();
            }
        }

        public void UpdateCurrentSelectedSlot(SlotPresenter slotPresenter)
        {
            this.currentSelectedSlot = slotPresenter;
        }

        public void DeActiveAllSlot() => this.entities.ForEach(e => e.DeActiveView());

    }
}