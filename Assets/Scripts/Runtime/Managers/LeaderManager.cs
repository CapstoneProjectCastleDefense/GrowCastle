namespace Runtime.Managers
{
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Leader;
    using Runtime.Enums;
    using Runtime.Managers.Base;
    using UnityEngine;

    public class LeaderManager : BaseElementManager<LeaderModel, LeaderPresenter, LeaderView>
    {
        private readonly LeaderBlueprint leaderBlueprint;
        public LeaderManager(BaseElementPresenter<LeaderModel, LeaderView, LeaderPresenter>.Factory factory, LeaderBlueprint leaderBlueprint) : base(factory)
        {
            this.leaderBlueprint = leaderBlueprint;
        }
        public override void Initialize() { }
        public void CreateSingleLeader(string id, Transform parent)
        {
            var leaderRecord = this.leaderBlueprint.GetDataById(id);
            this.CreateElement(new(
                id,
                leaderRecord.PrefabName,
                new()
                {
                    { StatEnum.Attack, (typeof(float), leaderRecord.BaseAttack) },
                    { StatEnum.Health, (typeof(float), leaderRecord.BaseHealth) },
                    { StatEnum.MaxHealth, (typeof(float), leaderRecord.BaseHealth) },
                    { StatEnum.MoveSpeed, (typeof(float), leaderRecord.BaseMoveSpeed) },
                    { StatEnum.AttackRange, (typeof(float), leaderRecord.AttackRange) },
                    { StatEnum.AttackSpeed, (typeof(float), leaderRecord.AttackSpeed) },
                    { StatEnum.AttackPriority, (typeof(AttackPriorityEnum), AttackPriorityEnum.Default) },
                },
                parent.position)).UpdateView().Forget();
        }
    }
}