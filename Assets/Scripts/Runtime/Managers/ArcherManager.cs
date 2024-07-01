namespace Runtime.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Archer.Base;
    using Runtime.Enums;
    using Runtime.Managers.Base;

    public class ArcherManager : BaseElementManager<ArcherModel, ArcherPresenter, ArcherView>
    {
        private readonly ArcherLocalDataController archerLocalDataController;
        private readonly ArcherBlueprint           archerBlueprint;
        private readonly CastleManager             castleManager;

        public ArcherManager(
            BaseElementPresenter<ArcherModel, ArcherView, ArcherPresenter>.Factory factory,
            ArcherLocalDataController archerLocalDataController,
            ArcherBlueprint archerBlueprint,
            CastleManager castleManager)
            : base(factory)
        {
            this.archerLocalDataController = archerLocalDataController;
            this.archerBlueprint           = archerBlueprint;
            this.castleManager             = castleManager;
        }

        public override void Initialize() { }

        public void CreateAllUnlockedArcher()
        {
            this.archerLocalDataController.GetAllUnlockedArcher().ForEach(this.CreateSingleArcher);
        }

        private void CreateSingleArcher(ArcherData archerData)
        {
            var archerSlot = this.castleManager.GetAllArcherSlot().First(e => e.index == archerData.index);
            var archerPresenter = this.CreateElement(new()
            {
                Index           = archerData.index,
                Level           = archerData.level,
                AddressableName = this.archerBlueprint.GetDataById(archerData.level).PrefabName,
                ParentView      = archerSlot.transform,
                Stats = new Dictionary<StatEnum, (Type, object)>
                {
                    { StatEnum.Attack, (typeof(float), 2f) },
                    { StatEnum.Health, (typeof(float), 10f) },
                    { StatEnum.AttackSpeed, (typeof(float), 1f) },
                    { StatEnum.AttackPriority, (typeof(AttackPriorityEnum), AttackPriorityEnum.Ground) }
                }
            });
            archerPresenter.UpdateView().Forget();
        }

        public void ChangeAttackStatusOfAllArcher(bool canAttack) { this.entities.ForEach(e => e.SetAttackStatus(canAttack)); }

        public void UpgradeArcher()
        {
            var newArcher = this.archerLocalDataController.UnlockArcher();
            var hasOldArcher = this.entities.Any(e => ((ArcherModel)e.GetModel()).Index == newArcher.index);
            if (hasOldArcher)
            {
                var archerPresenter = this.entities.First(e => ((ArcherModel)e.GetModel()).Index == newArcher.index);
                archerPresenter.Dispose();
                this.entities.Remove(archerPresenter);
            }
            this.CreateSingleArcher(newArcher);
        }
    }
}