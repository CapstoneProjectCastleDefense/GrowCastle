namespace Runtime.Elements.Entities.Summoner
{
    using System;
    using System.Collections.Generic;
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

    public class SummonerPresenter : BaseElementPresenter<SummonerModel, SummonerView, SummonerPresenter>, ISummonBeingPresenter
    {
        private const    string           AttackAnimName = "atk";
        private const    string           DeathAnimName  = "dead";
        private const    string           MoveAnimName   = "animation2";
        private readonly FindTargetSystem findTargetSystem;
        public SummonerPresenter(SummonerModel model, ObjectPoolManager objectPoolManager, FindTargetSystem findTargetSystem)
            : base(model, objectPoolManager)
        {
            this.findTargetSystem = findTargetSystem;
        }
        protected override UniTask<GameObject> CreateView() { return this.ObjectPoolManager.Spawn(this.Model.AddressableName); }

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            this.View.SkeletonAnimation.SetAnimation(MoveAnimName);
            this.View.HealthBarContainer.gameObject.SetActive(true);
            this.View.HealthBar.fillAmount = 1;
            this.View.transform.position   = this.Model.StartPos;
        }
        public override void Dispose() { }
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
                if (value == this.Model.GetStat<ITargetable>(StatEnum.TargetThatImLookingAt)) return;
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
        private void DoMove(Vector3 endPos, float distance)
        {
            if (this.TargetThatImAttacking == null || this.TargetThatImAttacking.IsDead) return;
            this.View.transform.DOKill();
            this.View.transform.DOMoveX(endPos.x, distance / this.Model.GetStat<float>(StatEnum.MoveSpeed));
        }
        public void Attack(ITargetable target) //TODO : Replace with a skill called attack
        {
            if (this.TargetThatImAttacking == null || this.TargetThatImAttacking.IsDead)
            {
                this.FindTarget();
                return;
            }

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

            return this.TargetThatImAttacking
                = this.TargetThatImAttacking is { IsDead: false }
                    ? this.TargetThatImAttacking
                    : this.TargetThatAttackingMe is { IsDead: false }
                        ? this.TargetThatAttackingMe
                        : this.findTargetSystem.GetTarget(this, priority, this.GetTags().ToList(), this.GetManagerTypes());
        }

        private void UpdateHealthView()
        {
            DOTween.Kill(this.View.HealthBar);
            this.View.HealthBar.DOFillAmount(this.Model.GetStat<float>(StatEnum.Health) / this.Model.GetStat<float>(StatEnum.MaxHealth), 0.1f);
        }
        public         float    AttackCooldownTime { get; private set; }
        public virtual Type[]   GetManagerTypes()  { return new[] { typeof(EnemyManager), typeof(CastleManager) }; }
        public virtual string[] GetTags()          { return new[] { "Fly", "Ground", "Boss", "Building" }; }
        public override void Tick()
        {
            base.Tick();
            if (!this.IsViewInit) return;
            if (this.IsDead)
            {
                this.View.transform.DOKill();
                return;
            }

            if (this.TargetThatImAttacking == null)
            {
                this.TargetThatImAttacking = this.FindTarget();
                return;
            }

            var endPos   = ((IElementPresenter)this.TargetThatImAttacking).GetView().transform.position;
            var distance = Vector3.Distance(this.View.transform.position, endPos);
            var range    = this.Model.GetStat<float>(StatEnum.AttackRange);
            if (distance > range)
                this.DoMove(endPos, distance);
            else
            {
                this.Attack(this.TargetThatImAttacking);
            }
        }
    }

    public class SummonerModel : IElementModel, IHaveStats
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public Vector3                              StartPos        { get; set; }
        public int                                  SortingIndex    { get; set; }
        public Dictionary<StatEnum, (Type, object)> Stats           { get; set; }
    }
}