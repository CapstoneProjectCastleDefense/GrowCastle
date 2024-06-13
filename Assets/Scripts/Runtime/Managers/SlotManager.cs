namespace Runtime.Managers
{
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Slot;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers.Base;

    public class SlotManager : BaseElementManager<SlotModel,SlotPresenter,SlotView>
    {
        private SlotPresenter currentSelectedSlot;
        public SlotManager(BaseElementPresenter<SlotModel, SlotView, SlotPresenter>.Factory factory)
            : base(factory)
        {
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

        public void UpdateCurrentSelectedSlot(SlotPresenter slotPresenter)
        {
            this.currentSelectedSlot = slotPresenter;
        }

    }
}