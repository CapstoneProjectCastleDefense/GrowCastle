namespace Runtime.Elements.Entities.Archer.Base
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Castles.ArcherSlots;
    using Runtime.Elements.EntitySkills;
    using Runtime.Elements.EntitySkills.ProjectileSkills;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;
    using Random = UnityEngine.Random;

    public class ArcherPresenter : BaseCombatantPresenter<ArcherModel, ArcherView, ArcherPresenter>, IArcherPresenter
    {
        private readonly EnemyManager      enemyManager;
        private readonly FindTargetSystem  findTargetSystem;
        private readonly EntitySkillSystem entitySkillSystem;
        private          bool              canAttack;

        protected ArcherPresenter(
            ArcherModel model,
            ObjectPoolManager objectPoolManager,
            EnemyManager enemyManager,
            FindTargetSystem findTargetSystem,
            EntitySkillSystem entitySkillSystem
        )
            : base(model, objectPoolManager)
        {
            this.enemyManager      = enemyManager;
            this.findTargetSystem  = findTargetSystem;
            this.entitySkillSystem = entitySkillSystem;
        }

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            Transform transform;
            (transform = this.View.transform).SetParent(this.Model.ParentView);
            transform.localPosition                             = Vector3.zero;
            this.View.GetComponent<MeshRenderer>().sortingOrder = this.Model.Index + 1;
            this.View.skeletonAnimation.GetComponent<MeshRenderer>().sortingOrder =
                this.View.GetComponentInParent<ArcherSlot>().GetComponent<SpriteRenderer>().sortingOrder;
            this.View.skeletonAnimation.ChaneSkeletonSkin(this.Model.Level.ToString());
        }

        public override void Tick()
        {
            if (!this.canAttack) return;
            if (this.AttackCooldownTime >= 1 / this.Model.GetStat<float>(StatEnum.AttackSpeed))
            {
                // var target = this.enemyManager.entities.Count > 0 ? this.enemyManager.entities.First() : null;
                if (this.TargetThatImLookingAt == null || this.TargetThatImLookingAt.IsDead)
                {
                    var target = this.FindTarget();
                    var random = Random.Range(100, 300);
                    this.TargetThatImLookingAt = target;
                    this.AttackCooldownTime    = -random * 1f / 1000;
                    UniTask.Delay(random).ContinueWith(() =>
                    {
                        this.Attack(this.TargetThatImLookingAt);
                    }).Forget();
                }
                else
                {
                    this.Attack(this.TargetThatImLookingAt);
                    this.AttackCooldownTime = 0;
                }
            }

            this.AttackCooldownTime += Time.deltaTime;
        }

        public void SetAttackStatus(bool attackStatus)
        {
            this.canAttack          = attackStatus;
            this.AttackCooldownTime = this.canAttack ? this.Model.GetStat<float>(StatEnum.AttackSpeed) : 0;
            if (!attackStatus) this.View.skeletonAnimation.SetAnimation("idle");
        }

        public void Attack(ITargetable target)
        {
            if (target == null) return;

            this.View.skeletonAnimation.SetAnimation("attack", false);
            this.entitySkillSystem.CastSkill(EntitySkillName.Arrow, new BaseProjectileSkillModel()
            {
                Id         = EntitySkillName.Arrow,
                StartPoint = this.View.spawnArrowPos.position,
                EndPoint   = target.GetGameObject().transform.position,
                Target     = target,
                Damage     = this.Model.GetStat<float>(StatEnum.Attack),
            });
        }

        public ITargetable FindTarget()
        {
            var priority = this.Model.GetStat<AttackPriorityEnum>(StatEnum.AttackPriority);
            if (priority == default)
            {
                priority = AttackPriorityEnum.Default;
                this.Model.SetStat(StatEnum.AttackPriority, priority);
            }

            var res = this.findTargetSystem.GetTarget(this, priority, this.GetTags().ToList(), this.GetManagerTypes(), 3);

            return res.Count > 0 ? res.RandomElement() : null;
        }

        public float AttackCooldownTime { get; private set; }

        public void CastSkill(string skillId, ITargetable target) { }

        public virtual Type[]   GetManagerTypes() { return new[] { typeof(EnemyManager), typeof(CastleManager) }; }
        public virtual string[] GetTags()         { return new[] { "Fly", "Ground", "Boss", "Building" }; }

        protected override UniTask<GameObject> CreateView() { return this.ObjectPoolManager.Spawn(this.Model.AddressableName); }

        public override void Dispose() { this.ObjectPoolManager.Recycle(this.View); }
    }
}