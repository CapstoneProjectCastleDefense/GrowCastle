namespace Runtime.Elements.Entities.Enemy
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using global::Extensions;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;
    using Runtime.Interfaces.Entities;
    using UnityEngine;

    public class EnemyPresenter : BaseElementPresenter<EnemyModel, EnemyView, EnemyPresenter>, IEnemyPresenter
    {
        private const           string AttackAnimName  = "atk";
        private const           string DeathAnimName   = "dead";
        protected EnemyPresenter(EnemyModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }

        public EnemyView GetEnemyView => this.View;

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            this.View.transform.position = this.Model.StartPos;
        }
        public void Attack(ITargetable target)
        {
            if (AttackAnimName.IsNullOrEmpty() && this.View.SkeletonAnimation) this.View.SkeletonAnimation.SetAnimation(AttackAnimName);
            target.OnGetHit(this.Model.GetStat<float>(StatEnum.Attack));
        }

        public ITargetable FindTarget() { return null; }

        public void OnGetHit(float damage)
        {
            var currentHealth = this.Model.GetStat<float>(StatEnum.Health);
            currentHealth -= damage;
            this.Model.SetStat(StatEnum.Health, currentHealth);
        }

        public void OnDeath()
        {
            var wait = 0f;
            if (DeathAnimName.IsNullOrEmpty()&& this.View.SkeletonAnimation!=null)
            {
                this.View.SkeletonAnimation.SetAnimation(DeathAnimName);
                wait = 0.3f;
            }
            UniTask.Delay(TimeSpan.FromSeconds(wait)).ContinueWith(this.Dispose).Forget();
        }

        protected override UniTask<GameObject> CreateView()
        {
            var res = this.ObjectPoolManager.Spawn(this.Model.AddressableName);
            return res;
        }

        public override void Dispose() { this.ObjectPoolManager.Recycle(this.View); }
    }
}