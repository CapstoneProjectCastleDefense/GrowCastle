﻿namespace Runtime.Elements.Entities.Enemy
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
        private const    string           MoveAnimName   = "animation2";
        private          EnemyManager     enemyManager;
        private readonly FindTargetSystem findTargetSystem;

        public Type[] GetManagerTypes() { return new[] { typeof(EnemyManager), typeof(CastleManager), typeof(LeaderManager) }; }

        protected EnemyPresenter(EnemyModel model, ObjectPoolManager objectPoolManager, FindTargetSystem findTargetSystem)
            : base(model, objectPoolManager)
        {
            this.findTargetSystem = findTargetSystem;
        }
        public void      SetManager(EnemyManager manager) => this.enemyManager = manager;

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            this.View.transform.position = this.Model.StartPos;
        }

        private void DoMove(Vector3 endPos, float distance)
        {
            if (this.TargetThatImAttacking == null) return;
            this.View.SkeletonAnimation.SetAnimation(MoveAnimName);
            this.View.transform.DOKill();
            this.View.transform.DOMove(endPos, distance / this.Model.GetStat<float>(StatEnum.MoveSpeed));
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
                        : this.findTargetSystem.GetTarget(this, priority, new() { "Ally", "Building" }, this.GetManagerTypes());
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
                this.View.SkeletonAnimation.SetAnimation(DeathAnimName, false);
                wait = this.View.SkeletonAnimation.AnimationState.GetCurrent(0).Animation.Duration;
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

        public override void Tick()
        {
            base.Tick();
            if(!this.IsViewInit) return;
            if (this.IsDead) return;
            if (this.TargetThatImAttacking == null)
            {
                this.TargetThatImAttacking = this.FindTarget();
                return;
            }

            var endPos = ((IElementPresenter)this.TargetThatImAttacking).GetView().transform.position;
            var distance = Vector3.Distance(this.View.transform.position, endPos);
            if (distance > 2f) //TODO: replace 2f with a variable
                this.DoMove(endPos, distance);
            else
            {
                this.View.transform.DOKill();
                this.Attack(this.TargetThatImAttacking);
            }
        }
    }
}