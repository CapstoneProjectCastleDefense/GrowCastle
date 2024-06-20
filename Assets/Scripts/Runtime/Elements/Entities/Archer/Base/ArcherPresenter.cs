namespace Runtime.Elements.Entities.Archer.Base
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Elements.EntitySkills;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using Runtime.Systems;
    using UnityEngine;

    public class ArcherPresenter : BaseElementPresenter<ArcherModel, ArcherView, ArcherPresenter>, IArcherPresenter
    {
        private readonly EnemyManager      enemyManager;
        private readonly FindTargetSystem  findTargetSystem;
        private readonly EntitySkillSystem entitySkillSystem;
        private          bool              canAttack;
        private          float             timer;
        protected ArcherPresenter(ArcherModel model,
            ObjectPoolManager objectPoolManager,
            EnemyManager enemyManager,
            FindTargetSystem findTargetSystem,
            EntitySkillSystem entitySkillSystem)
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
        }

        public override void Tick()
        {
            if (!this.canAttack) return;
            if (this.timer >= 1 / this.Model.GetStat<float>(StatEnum.AttackSpeed))
            {
                var target = this.enemyManager.entities.Count > 0 ? this.enemyManager.entities.First() : null;
                this.Attack(target);
                this.timer = 0;
            }

            this.timer += Time.deltaTime;
        }

        public void SetAttackStatus(bool attackStatus)
        {
            this.canAttack = attackStatus;
            this.timer     = this.canAttack ? this.Model.GetStat<float>(StatEnum.AttackSpeed) : 0;
        }

        public void Attack(ITargetable target = null)
        {
            target ??= this.FindTarget();
            if (target == null) return;
            Debug.Log("LVT - ArcherPresenter - target: " + (target as IElementPresenter).GetView().name);
            var enemy = (IElementPresenter)target;

            this.View.skeletonAnimation.SetAnimation("attack", false);
            this.entitySkillSystem.CastSkill("archer_normal_attack", new ProjectileSkillModel()
            {
                Id               = "archer_normal_attack",
                StartPoint       = this.View.spawnArrowPos.position,
                EndPoint         = enemy.GetView().transform.position
            });
        }

        public ITargetable FindTarget()
        {
            var priority = this.Model.GetStat<AttackPriorityEnum>(StatEnum.AttackPriority);

            var res = this.findTargetSystem.GetTarget(this, priority, new()
                {
                    AttackPriorityEnum.Ground.ToString(),
                    AttackPriorityEnum.Fly.ToString(),
                    AttackPriorityEnum.Boss.ToString(),
                    AttackPriorityEnum.Building.ToString()
                },
                this.GetManagerTypes());

            return res;
        }
        public float AttackCooldownTime { get; } = 0;

        public void CastSkill(string skillId, ITargetable target) { }

        public             Type[]              GetManagerTypes() { return new[] { typeof(EnemyManager), typeof(CastleManager) }; }
        protected override UniTask<GameObject> CreateView()      { return this.ObjectPoolManager.Spawn(this.Model.AddressableName); }

        public override void Dispose() { this.ObjectPoolManager.Recycle(this.View); }
    }
}