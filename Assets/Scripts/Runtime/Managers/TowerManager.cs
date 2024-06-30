namespace Runtime.Managers
{
    using Cysharp.Threading.Tasks;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Tower;
    using Runtime.Enums;
    using Runtime.Managers.Base;
    using UnityEngine;

    public class TowerManager : BaseElementManager<TowerModel, TowerPresenter, TowerView>
    {
        private readonly TowerLocalDataController towerLocalDataController;
        public TowerManager(BaseElementPresenter<TowerModel, TowerView, TowerPresenter>.Factory factory, TowerLocalDataController towerLocalDataController) : base(factory) {
            this.towerLocalDataController = towerLocalDataController;
        }

        public void CreateSingleTower(string id, Transform parent) {
            var towerRecord = this.towerLocalDataController.GetTowerRecord(id);
            this.CreateElement(new()
            {
                Id = id,
                ParentView = parent,
                Stats = new()
                {
                    { StatEnum.Attack, (typeof(float), towerRecord.BaseATK) },
                    { StatEnum.AttackSpeed, (typeof(float), towerRecord.ATKSPD) },
                    { StatEnum.AttackPriority,(typeof(AttackPriorityEnum), AttackPriorityEnum.Ground)}
                },
            }).UpdateView().Forget();
        }

        public void ChangeAttackStatusOfAllTower(bool canAttack) {
            this.entities.ForEach(e => e.SetAttackStatus(canAttack));
        }

        public override void Initialize() {

        }
    }
}