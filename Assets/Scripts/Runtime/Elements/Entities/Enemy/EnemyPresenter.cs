namespace Runtime.Elements.Entities.Enemy
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Extensions;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;
    using Runtime.Interfaces.Entities;
    using UnityEngine;

    public class EnemyPresenter : BaseElementPresenter<EnemyModel, EnemyView, EnemyPresenter>, IEnemyPresenter
    {
        private static readonly int    AttackAnimIndex = Animator.StringToHash(AttackAnimName);
        private static readonly int    DeathAnimIndex  = Animator.StringToHash(DeathAnimName);
        private const           string AttackAnimName  = "Attack";
        private const           string DeathAnimName   = "Death";
        protected EnemyPresenter(EnemyModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }

        public void Attack(ITargetable target)
        {
            if (AttackAnimName.IsNullOrEmpty() && this.View.Animator) this.View.Animator.SetTrigger(AttackAnimIndex);
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
            if (DeathAnimName.IsNullOrEmpty()&& this.View.Animator)
            {
                this.View.Animator.SetTrigger(DeathAnimIndex);
                wait = this.View.Animator.runtimeAnimatorController.animationClips.First(x => x.name == DeathAnimName).length;
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