namespace Runtime.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities;
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
        private readonly ArcherConfigBlueprint     archerConfigBlueprint;
        private readonly TalentLocalDataController talentLocalDataController;
        private readonly TalentBlueprint           talentBlueprint;

        public ArcherManager(
            BaseElementPresenter<ArcherModel, ArcherView, ArcherPresenter>.Factory factory,
            ArcherLocalDataController                                              archerLocalDataController,
            ArcherBlueprint                                                        archerBlueprint,
            CastleManager                                                          castleManager,
            ArcherConfigBlueprint                                                  archerConfigBlueprint,
            TalentLocalDataController                                              talentLocalDataController,
            TalentBlueprint                                                        talentBlueprint
        )
            : base(factory)
        {
            this.archerLocalDataController = archerLocalDataController;
            this.archerBlueprint           = archerBlueprint;
            this.castleManager             = castleManager;
            this.archerConfigBlueprint     = archerConfigBlueprint;
            this.talentLocalDataController = talentLocalDataController;
            this.talentBlueprint           = talentBlueprint;
        }

        public override void Initialize() { }

        public void CreateAllUnlockedArcher() { this.archerLocalDataController.GetAllUnlockedArcher().ForEach(this.CreateSingleArcher); }

        private void CreateSingleArcher(ArcherData archerData)
        {
            var archerSlot      = this.castleManager.GetAllArcherSlot().First(e => e.index == archerData.index);
            var baseAttackSpeed = this.archerConfigBlueprint.BaseAttackSpeed;
            var archerPresenter = this.CreateElement(new()
            {
                Index           = archerData.index,
                Level           = archerData.level,
                AddressableName = this.archerConfigBlueprint.DefaultPrefabName,
                ParentView      = archerSlot.transform,
                BaseStats = new()
                {
                    { StatEnum.Attack, (typeof(float), this.archerConfigBlueprint.BaseDamage + this.archerConfigBlueprint.Coefficient * archerData.level) },
                    { StatEnum.Health, (typeof(float), 10f) },
                    {
                        StatEnum.AttackSpeed,
                        (typeof(float),
                            baseAttackSpeed
                            + this.talentBlueprint[TalentType.IncreaseArcherAttackSpeed]
                                .TalentLevelToDataRecords[this.talentLocalDataController.GetTalentLevel(TalentType.IncreaseArcherAttackSpeed)].EffectValue
                            / 100
                            * baseAttackSpeed
                            + this.archerConfigBlueprint.Coefficient * archerData.level)
                    },
                    { StatEnum.AttackPriority, (typeof(AttackPriorityEnum), AttackPriorityEnum.Boss) }
                }
            });
            archerPresenter.UpdateView().Forget();
        }

        public void ChangeAttackStatusOfAllArcher(bool canAttack) { this.entities.ForEach(e => e.SetAttackStatus(canAttack)); }

        public void UpgradeArcher()
        {
            var newArcher = this.archerLocalDataController.UnlockArcher();

            if (newArcher == null)
            {
                AudioService.Instance.PlaySound("Error");

                return;
            }

            AudioService.Instance.PlaySound("Cash");

            var hasOldArcher = this.entities.Any(e => e.Model.Index == newArcher.index);
            if (hasOldArcher)
            {
                var archerPresenter = this.entities.First(e => e.Model.Index == newArcher.index);
                archerPresenter.Dispose();
                this.entities.Remove(archerPresenter);
            }
            this.castleManager.OnArcherUpgrade();
            this.CreateSingleArcher(newArcher);
        }

        public void UpdateStatAllArcher()
        {
            var baseAttackSpeed = this.archerConfigBlueprint.BaseAttackSpeed;
            this.entities.ForEach(archer =>
            {
                var baseArcherAttack      = this.archerConfigBlueprint.BaseDamage + this.archerConfigBlueprint.Coefficient * archer.Model.Level;
                var baseArcherAttackSpeed = this.archerConfigBlueprint.BaseAttackSpeed + this.archerConfigBlueprint.Coefficient * archer.Model.Level;
                archer.Model.BaseStats = new()
                {
                    {
                        StatEnum.Attack, (typeof(float), baseArcherAttack
                            + this.talentLocalDataController.GetTalentEffect(TalentType.IncreaseArcherAttack)
                            * baseAttackSpeed)
                    },
                    { StatEnum.Health, (typeof(float), 10f) },
                    {
                        StatEnum.AttackSpeed,
                        (typeof(float), baseArcherAttackSpeed
                            + this.talentLocalDataController.GetTalentEffect(TalentType.IncreaseArcherAttackSpeed)
                            * baseArcherAttackSpeed)
                    },
                    { StatEnum.AttackPriority, (typeof(AttackPriorityEnum), AttackPriorityEnum.Ground) }
                };
            });
        }
    }
}