namespace Runtime.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Archer.Base;
    using Runtime.Elements.Entities.Castles.ArcherSlots;
    using Runtime.Enums;
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
                var archerPresenter = this.CreateElement(new()
                {
                    Index = archerData.index,
                    Level = archerData.level,
                    AddressableName = this.archerBlueprint.GetDataById(archerData.level).PrefabName,
                    ParentView = archerSlot.gameObject.transform,
                    Stats = new Dictionary<StatEnum, (Type, object)>
                    {
                        { StatEnum.Attack, (typeof(float), 2f) },
                        { StatEnum.Health, (typeof(float), 10f) },
                        { StatEnum.AttackSpeed, (typeof(float), 1f) },
                    }
                });
                archerPresenter.UpdateView().Forget();
            });
        }

        public void ChangeAttackStatusOfAllArcher(bool canAttack)
        {
            this.entities.ForEach(e=>e.SetAttackStatus(canAttack));
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