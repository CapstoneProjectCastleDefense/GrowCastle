namespace Runtime.Elements.Entities.Enemy
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using global::Extensions;
    using Models.LocalData;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using Runtime.Systems;
    using TMPro;
    using UnityEngine;

    public class EnemyPresenter : BaseCombatantPresenter<EnemyModel, EnemyView, EnemyPresenter>, IEnemyPresenter
    {
        private string AttackAnimName => this.View.attackAnimations.RandomElement();
        private string DeathAnimName  => this.View.deathAnimations.RandomElement();
        private string MoveAnimName   => this.View.moveAnimation;

        private readonly FindTargetSystem            findTargetSystem;
        private readonly ResourceLocalDataController resourceLocalDataController;

        public virtual Type[]   GetManagerTypes() { return new[] { typeof(EnemyManager), typeof(CastleManager), typeof(LeaderManager) }; }
        public virtual string[] GetTags()         { return new[] { "Ally", "Building" }; }

        protected EnemyPresenter(
            EnemyModel model,
            ObjectPoolManager objectPoolManager,
            FindTargetSystem findTargetSystem,
            ResourceLocalDataController resourceLocalDataController)
            : base(model, objectPoolManager)
        {
            this.findTargetSystem            = findTargetSystem;
            this.resourceLocalDataController = resourceLocalDataController;
        }
        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            this.View.SkeletonAnimation.SetAnimation(this.MoveAnimName);
            this.View.HealthBarContainer.gameObject.SetActive(true);
            this.View.HealthBar.fillAmount                                        = 1;
            this.View.transform.position                                          = this.Model.StartPos + Vector3.up * (this.View.tag.Equals("Fly") ? 5 : 0);
            this.View.SkeletonAnimation.GetComponent<MeshRenderer>().sortingOrder = (int)((this.Model.StartPos.y + 10) * -100);
            this.View.Rigidbody2D.constraints                                     = RigidbodyConstraints2D.None;
        }


        private void DoMove(float range, float distance)
        {
            if (distance <= range)
            {
                this.View.Rigidbody2D.velocity = Vector2.zero;
                return;
            }

            this.View.Rigidbody2D.velocity = new Vector2(-1 * this.Model.GetStat<float>(StatEnum.MoveSpeed), 0);
        }

        public void Attack(ITargetable target) //TODO : Replace with a skill called attack
        {
            if (!AttackAnimName.IsNullOrEmpty() &&
                this.View.SkeletonAnimation &&
                Time.time >= this.AttackCooldownTime)
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
                        : this.findTargetSystem.GetTarget(this, priority, this.GetTags().ToList(), this.GetManagerTypes(), 1).FirstOrDefault();
        }

        public float AttackCooldownTime { get; private set; }

        private void UpdateHealthView()
        {
            DOTween.Kill(this.View.HealthBar);
            this.View.HealthBar.DOFillAmount(this.Model.GetStat<float>(StatEnum.Health) / this.Model.GetStat<float>(StatEnum.MaxHealth), 0.1f);
        }

        public override void UpdateStats()
        {
            base.UpdateStats();
            this.UpdateHpStat();
        }

        private void UpdateHpStat()
        {
            if (this.IsDead) return;
            var currentHealth = this.Model.GetStat<float>(StatEnum.Health);

            this.Model.SetStat(StatEnum.Health, currentHealth);
            if (currentHealth <= 0)
                this.OnDeath();
            else
                this.UpdateHealthView();
        }

        public void OnDeath()
        {
            if (this.IsDead) return;
            this.View.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePosition;
            float goldDrop = this.Model.GetStat<float>(StatEnum.Gold);
            this.resourceLocalDataController.ReceiveResource(ResourceType.Gold, goldDrop);

            this.TargetThatImLookingAt = null;
            this.IsDead                = true;

            this.View.HealthBarContainer.gameObject.SetActive(false);

            this.DropCoin();

            var wait = 0f;
            if (!DeathAnimName.IsNullOrEmpty() &&
                this.View.SkeletonAnimation != null)
            {
                this.View.SkeletonAnimation.SetAnimation(DeathAnimName, false);
                wait = this.View.SkeletonAnimation.AnimationState.GetCurrent(0).Animation.Duration;
            }

            this.View.transform.DOKill();

            UniTask.Delay(TimeSpan.FromSeconds(wait)).ContinueWith(this.Dispose).Forget();
        }

        private void DropCoin()
        {
            var goldDrop = this.Model.GetStat<float>(StatEnum.Gold);
            var expDrop  = this.Model.GetStat<float>(StatEnum.Exp);
            this.resourceLocalDataController.ReceiveResource(ResourceType.Gold, goldDrop);
            this.resourceLocalDataController.ReceiveResource(ResourceType.Exp, expDrop);
            this.CoinPopUp(goldDrop);
        }

        private void CoinPopUp(float goldDrop)
        {
            this.View.CoinPopupCanvas.alpha                                    = 0;
            this.View.CoinPopup.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 5.55f);
            this.View.CoinPopup.GetComponentInChildren<TextMeshProUGUI>().SetText("+ " + goldDrop);
            this.View.CoinPopup.SetActive(true);
            this.View.CoinPopup.GetComponent<RectTransform>().DOAnchorPosY(6.55f, 0.3f);
            this.View.CoinPopupCanvas.DOFade(1f, 0.3f).OnComplete(() => { this.View.CoinPopupCanvas.DOFade(0f, 0.3f); });
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

        protected override UniTask<GameObject> CreateView()
        {
            var res = this.ObjectPoolManager.Spawn(this.Model.AddressableName);
            return res;
        }

        public override void Dispose()
        {
            this.ObjectPoolManager.Recycle(this.View);
            this.ElementManager.entities.Remove(this);
            ((EnemyManager)this.ElementManager).UpdateEnemyDeathCounter();
        }

        public override void Tick()
        {
            base.Tick();
            if (!this.IsViewInit) return;
            if (this.IsDead)
            {
                this.View.Rigidbody2D.velocity = Vector2.zero;
                return;
            }

            if (this.TargetThatImAttacking == null ||
                this.TargetThatImAttacking.IsDead)
            {
                this.TargetThatImLookingAt = this.FindTarget();
            }

            if (this.TargetThatImLookingAt == null) return;
            var endPos   = this.TargetThatImLookingAt.GetGameObject().transform.position;
            var distance = Vector3.Distance(this.View.transform.position, endPos);
            var range    = this.Model.GetStat<float>(StatEnum.AttackRange);
            if (distance > range)
                this.DoMove(range, distance);
            else
            {
                this.View.Rigidbody2D.velocity = Vector2.zero;
                this.Attack(this.TargetThatImLookingAt);
            }
        }
    }
}