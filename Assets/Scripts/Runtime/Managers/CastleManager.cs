namespace Runtime.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Castles;
    using Runtime.Elements.Entities.Castles.ArcherSlots;
    using Runtime.Managers.Base;

    public class CastleManager : BaseElementManager<CastleModel,CastlePresenter,CastleView>
    {
        private readonly CastleLocalDataController castleLocalDataController;
        private readonly SlotManager slotManager;

        public CastleManager(BaseElementPresenter<CastleModel, CastleView, CastlePresenter>.Factory factory, CastleLocalDataController castleLocalDataController, SlotManager slotManager)
            : base(factory) {
            this.castleLocalDataController = castleLocalDataController;
            this.slotManager = slotManager;
        }
        public override void Initialize()
        {

        }
        public void UpgradeCastle()
        {
            this.castleLocalDataController.UpgradeCastle();
            this.entities.First().UpdateBlockBaseOnCurrentLevel();
            this.slotManager.UpdateAllSlotsBaseOnCurrentLevel();
        }

        public List<ArcherSlot> GetAllArcherSlot() => this.entities[0].CastleView.listArcherSlot;
    }
}