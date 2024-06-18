namespace Runtime.Elements.Entities.Leader
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using UnityEngine;

    public class LeaderPresenter : BaseElementPresenter<LeaderModel, LeaderView, LeaderPresenter>, ILeaderPresenter
    {
        protected LeaderPresenter(LeaderModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }

        public void Attack(ITargetable target) { throw new NotImplementedException(); }

        public ITargetable FindTarget()      { throw new NotImplementedException(); }
        public Type[]      GetManagerTypes() { return new[] { typeof(Managers.CastleManager), typeof(Managers.EnemyManager) }; }

        public void CastSkill(string skillId, ITargetable target) { throw new NotImplementedException(); }

        public void Equip(IEquipment equipment) { throw new NotImplementedException(); }

        public void UnEquip(IEquipment equipment) { throw new NotImplementedException(); }

        public void OnGetHit(float damage) { throw new NotImplementedException(); }

        public void OnDeath() { throw new NotImplementedException(); }

        public ITargetable TargetThatImAttacking
        {
            get => this.Model.GetStat<ITargetable>(StatEnum.TargetThatImAttacking);
            set
            {
                if (value == this.Model.GetStat<ITargetable>(StatEnum.TargetThatImAttacking)) return;
                this.Model.SetStat(StatEnum.TargetThatImAttacking, value);
            }
        }

        public ITargetable TargetThatAttackingMe
        {
            get => this.Model.GetStat<ITargetable>(StatEnum.TargetThatAttackingMe);
            set
            {
                if (value == this.Model.GetStat<ITargetable>(StatEnum.TargetThatAttackingMe)) return;
                this.Model.SetStat(StatEnum.TargetThatAttackingMe, value);
            }
        }

        public             bool                IsDead       => this.Model.GetStat<float>(StatEnum.Health) <= 0;
        protected override UniTask<GameObject> CreateView() { throw new NotImplementedException(); }
        public override    void                Dispose()    { throw new NotImplementedException(); }
    }
}