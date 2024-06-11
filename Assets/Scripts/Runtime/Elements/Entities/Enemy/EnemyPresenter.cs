namespace Runtime.Elements.Entities.Enemy
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;
    using Runtime.Interfaces.Entities;
    using UnityEngine;

    public class EnemyPresenter : BaseElementPresenter<EnemyModel, EnemyView, EnemyPresenter>, IEnemyPresenter
    {
        private const string AttackAnimName = "Attack";
        private const string DeathAnimName  = "Death";
        protected EnemyPresenter(EnemyModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }

        public void Attack(ITargetable target)
        {
            this.View.Animator.SetTrigger(AttackAnimName);
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
            this.View.Animator.SetTrigger(DeathAnimName);
            var clip = this.View.Animator.runtimeAnimatorController.animationClips.First(x => x.name == DeathAnimName);
            UniTask.Delay(TimeSpan.FromSeconds(clip.length)).ContinueWith(this.Dispose).Forget();
        }

        protected override UniTask<GameObject> CreateView()
        {
            var res = this.ObjectPoolManager.Spawn(this.Model.AddressableName);
            return res;
        }

        public override void Dispose() { this.ObjectPoolManager.Recycle(this.View); }
    }
}