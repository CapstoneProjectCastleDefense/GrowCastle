namespace Runtime.Elements.Entities.Enemy
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using global::Extensions;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using Runtime.Systems;
    using UnityEngine;

    public class EnemyPresenter : BaseElementPresenter<EnemyModel, EnemyView, EnemyPresenter>, IEnemyPresenter
    {
        private const    string           AttackAnimName = "atk";
        private const    string           DeathAnimName  = "dead";
        private          EnemyManager     enemyManager;
        private readonly FindTargetSystem findTargetSystem;
        protected EnemyPresenter(EnemyModel model, ObjectPoolManager objectPoolManager, FindTargetSystem findTargetSystem)
            : base(model, objectPoolManager)
        {
            this.findTargetSystem = findTargetSystem;
        }

        public EnemyView GetEnemyView                     => this.View;
        public void      SetManager(EnemyManager manager) => this.enemyManager = manager;

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            this.View.transform.position = this.Model.StartPos;
        }

        private void StartMove()
        {
            this.View.transform.DOMove(this.Model.EndPos, 1 / this.Model.GetStat<float>(StatEnum.MoveSpeed)).onComplete += () =>
            {
                //start attack
            };
        }
        public void Attack(ITargetable target)
        {
            if (!AttackAnimName.IsNullOrEmpty() && this.View.SkeletonAnimation) this.View.SkeletonAnimation.SetAnimation(AttackAnimName);
            target.OnGetHit(this.Model.GetStat<float>(StatEnum.Attack));
        }

        public ITargetable FindTarget()
        {
            var priority = this.Model.GetStat<AttackPriorityEnum>(StatEnum.AttackPriority);
            if (priority == default)
            {
                priority = AttackPriorityEnum.Default;
                this.Model.SetStat(StatEnum.AttackPriority, priority);
            }

            return this.TargetThatImAttacking
                = this.TargetThatImAttacking is { IsDead: false }
                    ? this.TargetThatImAttacking
                    : this.TargetThatAttackingMe is { IsDead: false }
                        ? this.TargetThatAttackingMe
                        : this.findTargetSystem.GetTarget(this, priority, new() { "Ally", "Building" });
        }

        private void UpdateHealthView()
        {
            DOTween.Kill(this.View.HealthBar);
            this.View.HealthBar.DOFillAmount(this.Model.GetStat<float>(StatEnum.Health) / 20, 0.1f);
        }
        public void OnGetHit(float damage)
        {
            var currentHealth = this.Model.GetStat<float>(StatEnum.Health);
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
            }

            this.Model.SetStat(StatEnum.Health, currentHealth);
            this.UpdateHealthView();
            if (currentHealth <= 0) this.OnDeath();
        }

        public void OnDeath()
        {
            var wait = 0f;
            if (!DeathAnimName.IsNullOrEmpty() && this.View.SkeletonAnimation != null)
            {
                this.View.SkeletonAnimation.SetAnimation(DeathAnimName);
                wait = 1f;
            }

            UniTask.Delay(TimeSpan.FromSeconds(wait)).ContinueWith(this.Dispose).Forget();
        }

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

        public bool IsDead => this.Model.GetStat<float>(StatEnum.Health) <= 0;

        protected override UniTask<GameObject> CreateView()
        {
            var res = this.ObjectPoolManager.Spawn(this.Model.AddressableName);
            return res;
        }

        public override void Dispose()
        {
            this.ObjectPoolManager.Recycle(this.View);
            this.enemyManager.entities.Remove(this);
        }
    }
}