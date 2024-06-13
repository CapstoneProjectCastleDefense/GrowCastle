namespace Runtime.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Archer.Base;
    using Runtime.Elements.Entities.Castles.ArcherSlots;
    using Runtime.Managers.Base;

    public class ArcherManager : BaseElementManager<ArcherModel,ArcherPresenter,ArcherView>
    {
        private readonly ArcherLocalDataController archerLocalDataController;
        private readonly ArcherBlueprint           archerBlueprint;

        public ArcherManager(BaseElementPresenter<ArcherModel, ArcherView, ArcherPresenter>.Factory factory, ArcherLocalDataController archerLocalDataController, ArcherBlueprint archerBlueprint) : base(factory)
        {
            this.archerLocalDataController = archerLocalDataController;
            this.archerBlueprint           = archerBlueprint;
        }

        public override void Initialize()
        {

        }

        public void CreateAllUnlockedArcher(List<ArcherSlot> archerSlots)
        {
            this.archerLocalDataController.GetAllUnlockedArcher().ForEach(archerData =>
            {
                var archerSlot      = archerSlots.First(e => e.index == archerData.index);
                var archerPresenter = this.CreateElement(new() { Index = archerData.index, Level = archerData.level, AddressableName = this.archerBlueprint.GetDataById(archerData.level).PrefabName,ParentView = archerSlot.gameObject.transform});
                archerPresenter.UpdateView().Forget();
            });
        }

        public void UpgradeArcher()
        {
            this.archerLocalDataController.UnlockArcher();
        }

        public override void DisposeAllElement()
        {

        }
    }
}