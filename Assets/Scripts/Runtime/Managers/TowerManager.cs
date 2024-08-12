namespace Runtime.Managers
{
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Tower;
    using Runtime.Enums;
    using Runtime.Managers.Base;
    using Runtime.Services;
    using UnityEngine;

    public class TowerManager : BaseElementManager<TowerModel, TowerPresenter, TowerView>
    {
        private readonly HeroBlueprint           heroBlueprint;
        private readonly HeroLocalDataController heroLocalDataController;
        private readonly HeroConfigBlueprint     heroConfigBlueprint;
        private readonly HeroUpgradeService      heroUpgradeService;
        public TowerManager(BaseElementPresenter<TowerModel, TowerView, TowerPresenter>.Factory factory, HeroBlueprint heroBlueprint,
            HeroLocalDataController heroLocalDataController, HeroConfigBlueprint heroConfigBlueprint, HeroUpgradeService heroUpgradeService) : base(factory)
        {
            this.heroBlueprint           = heroBlueprint;
            this.heroLocalDataController = heroLocalDataController;
            this.heroConfigBlueprint     = heroConfigBlueprint;
            this.heroUpgradeService      = heroUpgradeService;
        }

        public void CreateSingleTower(string id, Transform parent)
        {
            var towerRecord = this.heroConfigBlueprint.GetDataById(id);
            var towerPresenter = this.CreateElement(new()
            {
                Id         = id,
                ParentView = parent,
                BaseStats = new()
                {
                    { StatEnum.Attack, (typeof(float), this.heroUpgradeService.GetCurrentAttack(id)) },
                    { StatEnum.AttackSpeed, (typeof(float), towerRecord.BaseAttackSpeed) },
                    { StatEnum.AttackPriority, (typeof(AttackPriorityEnum), AttackPriorityEnum.Ground) }
                },
            }).UpdateView();
            towerPresenter.Forget();
        }

        public void ChangeAttackStatusOfAllTower(bool canAttack) { this.entities.ForEach(e => e.SetAttackStatus(canAttack)); }

        public override void Initialize() { }
    }
}