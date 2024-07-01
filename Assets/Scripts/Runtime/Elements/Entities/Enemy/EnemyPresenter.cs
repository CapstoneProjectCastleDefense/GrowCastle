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
        private const    string           MoveAnimName   = "animation2";
        private          EnemyManager     enemyManager;
        private readonly FindTargetSystem findTargetSystem;

        public virtual Type[] GetManagerTypes() { return new[] { typeof(EnemyManager), typeof(CastleManager), typeof(LeaderManager) }; }
        public virtual string[] GetTags() { return new[] { "Ally", "Building" }; }

        protected EnemyPresenter(EnemyModel model, ObjectPoolManager objectPoolManager, FindTargetSystem findTargetSystem)
            : base(model, objectPoolManager)
        {
            this.findTargetSystem = findTargetSystem;
        }
        public void SetManager(EnemyManager manager) => this.enemyManager = manager;

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            this.View.SkeletonAnimation.SetAnimation(MoveAnimName);
            this.View.HealthBarContainer.gameObject.SetActive(true);
            this.View.HealthBar.fillAmount = 1;
            this.View.transform.position   = this.Model.StartPos;
        }

        private bool isMoving;

        private void DoMove(Vector3 endPos, float distance)
        {
            if(this.isMoving) return;
            this.isMoving = true;
            this.View.transform.DOKill();
            this.View.transform.DOMoveX(endPos.x, distance / this.Model.GetStat<float>(StatEnum.MoveSpeed));
        }
        public void Attack(ITargetable target) //TODO : Replace with a skill called attack
        {
            this.isMoving = false;
            if (!AttackAnimName.IsNullOrEmpty() && this.View.SkeletonAnimation && Time.time >= this.AttackCooldownTime)
            {
                this.View.transform.DOKill();
                this.View.SkeletonAnimation.SetAnimation(AttackAnimName);
                target.OnGetHit(this.Model.GetStat<float>(StatEnum.Attack));
                target.TargetThatAttackingMe = this;
                var attackSpeed                   = this.Model.GetStat<float>(StatEnum.AttackSpeed);
                if (attackSpeed <= 0) attackSpeed = 1f / this.View.SkeletonAnimation.AnimationState.GetCurrent(0).Animation.Duration;
                this.AttackCooldownTime = Time.time + 1f / attackSpeed;
            }
        }

        public ITargetable FindTarget()
        {
            var priority = this.Model.GetStat<AttackPriorityEnum>(StatEnum.AttackPriority);
            if (priority == default)
            {
                priority = AttackPriorityEnum.Default;
                this.Model.SetStat(StatEnum.AttackPriority, priority);
            }

            return this.TargetThatImAttacking is { IsDead: false }
                    ? this.TargetThatImAttacking
                    : this.TargetThatAttackingMe is { IsDead: false }
                        ? this.TargetThatAttackingMe
                        : this.TargetThatImLookingAt is { IsDead: false }
                            ? this.TargetThatImLookingAt
                            : this.findTargetSystem.GetTarget(this, priority, this.GetTags().ToList(), this.GetManagerTypes());
        }
        public float AttackCooldownTime { get; private set; }

        private void UpdateHealthView()
        {
            DOTween.Kill(this.View.HealthBar);
            this.View.HealthBar.DOFillAmount(this.Model.GetStat<float>(StatEnum.Health) / this.Model.GetStat<float>(StatEnum.MaxHealth), 0.1f);
        }
        public void OnGetHit(float damage)
        {
            if (this.IsDead) return;
            var currentHealth = this.Model.GetStat<float>(StatEnum.Health);
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
            }

            this.Model.SetStat(StatEnum.Health, currentHealth);
            if (currentHealth <= 0)
                this.OnDeath();
            else
                this.UpdateHealthView();
        }

        public void OnDeath()
        {
            if (this.IsDead) return;
            this.IsDead = true;
            this.View.HealthBarContainer.gameObject.SetActive(false);
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
        
        public ITargetable TargetThatImLookingAt
        {
            get => this.Model.GetStat<ITargetable>(StatEnum.TargetThatImLookingAt);
            set
            {
                if(value == this.Model.GetStat<ITargetable>(StatEnum.TargetThatImLookingAt)) return;
                this.Model.SetStat(StatEnum.TargetThatImLookingAt, value);
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

        public bool IsDead { get; private set; }

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
            if (!this.IsViewInit) return;
            if (this.IsDead)
            {
                this.View.transform.DOKill();
                return;
            }

            if (this.TargetThatImAttacking == null || this.TargetThatImAttacking.IsDead)
            {
                this.TargetThatImLookingAt = this.FindTarget();
            }

            var endPos   = ((IElementPresenter)this.TargetThatImLookingAt).GetView().transform.position;
            var distance = Vector3.Distance(this.View.transform.position, endPos);
            var range    = this.Model.GetStat<float>(StatEnum.AttackRange);
            if (distance > range)
                this.DoMove(endPos, distance);
            else
            {
                this.Attack(this.TargetThatImLookingAt);
            }
        }
    }
}