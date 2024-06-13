namespace Runtime.Managers
{
    using Cysharp.Threading.Tasks;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Slot;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers.Base;

    public class SlotManager : BaseElementManager<SlotModel,SlotPresenter,SlotView>
    {
        private readonly SlotLocalDataController slotLocalDataController;
        private          SlotPresenter           currentSelectedSlot;
        public SlotManager(BaseElementPresenter<SlotModel, SlotView, SlotPresenter>.Factory factory, SlotLocalDataController slotLocalDataController)
            : base(factory)
        {
            this.slotLocalDataController = slotLocalDataController;
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
            this.slotLocalDataController.GetAllSlotData.ForEach(slotData =>
            {
                var slotPresenter = this.CreateElement(new SlotModel() { AddressableName = "BaseSlot", Id = slotData.SlotId.ToString(), SlotRecord = this.slotLocalDataController.GetSlotDataRecord(slotData.SlotId) });
                slotPresenter.UpdateView().Forget();
            });
        }

        public void UpdateCurrentSelectedSlot(SlotPresenter slotPresenter)
        {
            this.currentSelectedSlot = slotPresenter;
        }

    }
}