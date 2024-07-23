namespace Runtime.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Castles;
    using Runtime.Elements.Entities.Castles.ArcherSlots;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Managers.Base;
    using Sirenix.Utilities;
    using Zenject;

    public class CastleManager : BaseElementManager<CastleModel,CastlePresenter,CastleView>
    {
        private readonly CastleLocalDataController castleLocalDataController;
        private readonly SlotManager               slotManager;
        private readonly SignalBus                 signalBus;

        public CastleManager(BaseElementPresenter<CastleModel, CastleView, CastlePresenter>.Factory factory, CastleLocalDataController castleLocalDataController, SlotManager slotManager,SignalBus signalBus)
            : base(factory) {
            this.castleLocalDataController = castleLocalDataController;
            this.slotManager               = slotManager;
            this.signalBus                 = signalBus;
        }
        public override void Initialize()
        {

        }
        public void UpgradeCastle()
        {
            bool canUpgrade = this.castleLocalDataController.UpgradeCastle();
            if (canUpgrade)
            {
                this.entities.First().OnUpgrade();
                this.slotManager.UpdateAllSlots();
            }
        }
        public bool UseManaForSkill(float manaValue)
        {
            return this.entities.First().UseManaForSkill(manaValue);
        }
        

        public void ResetCurrentCastleHealthAndMana()
        {
            if(this.entities.Count==0) return;
            this.entities.First().ResetHealthAndMana();
        }

        public List<ArcherSlot> GetAllArcherSlot() => this.entities[0].CastleView.listArcherSlot;

        public void OnArcherUpgrade() {
            this.entities.First().ArcherUpgradePopUp();
        }

        public void UpdateStatForCurrentCastle()
        {
            if(this.entities.Count==0) return;
            this.entities.First().UpdateStat();
        }
    }
}