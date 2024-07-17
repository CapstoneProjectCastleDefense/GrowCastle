namespace Runtime.Elements.Entities.Archer.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Tags;
    using Runtime.Combat.CombatActions;
    using Runtime.Combat.CombatActions.Models;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Castles.ArcherSlots;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using Runtime.Systems;
    using UnityEngine;

    public class ArcherPresenter : BaseCombatantPresenter<ArcherModel, ArcherView, ArcherPresenter>, IArcherPresenter
    {
        private readonly EnemyManager         enemyManager;
        private readonly FindTargetSystem     findTargetSystem;
        private readonly CombatActionExecutor combatActionExecutor;
        private          bool                 canAttack;
        private          float                attackCooldown;

        protected ArcherPresenter(
            ArcherModel model,
            ObjectPoolManager objectPoolManager,
            EnemyManager enemyManager,
            FindTargetSystem findTargetSystem,
            CombatActionExecutor combatActionExecutor
        )
            : base(model, objectPoolManager)
        {
            this.enemyManager         = enemyManager;
            this.findTargetSystem     = findTargetSystem;
            this.combatActionExecutor = combatActionExecutor;
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
            this.CooldownAttack();
            if (this.attackCooldown > 0) return;

            if (this.Attack())
            {
                this.attackCooldown = 1 / this.Model.Stats.GetStat<float>(StatEnum.AttackSpeed);
            }
        }

        private void CooldownAttack()
        {
            //check if has haste buff, reduce cooldown more
            var stats      = this.Model.Stats;
            var reduceTime = Time.deltaTime;
            if (stats.TryGetValue(StatEnum.HasteBuff, out (Type type, object value) param))
            {
                reduceTime *= (float)param.value;
            }

            this.attackCooldown -= reduceTime;
        }

        private bool Attack()
        {
            var target = this.FindTarget(this);

            if (target == null) return false;
            var damage = this.Model.GetStat<float>(StatEnum.Attack);
            this.combatActionExecutor.Execute(CombatActionId.FireProjectile, new FireProjectileModel("archer_normal_attack",
                                                                                                     target,
                                                                                                     this,
                                                                                                     new List<IEffectTag>()
                                                                                                     {
                                                                                                         new InstantDamageTag()
                                                                                                         {
                                                                                                             Damage = damage
                                                                                                         }
                                                                                                     }));
            return true;
        }

        public ITargetable FindTarget(ICombatantPresenter caster)
        {
            var stats    = this.Model.Stats;
            var priority = stats.GetStat<AttackPriorityEnum>(StatEnum.AttackPriority);
            if (priority == default)
            {
                stats.SetStat(StatEnum.AttackPriority, AttackPriorityEnum.Default);
            }

            var target = this.findTargetSystem.GetTarget(caster, priority, this.GetTags().ToList(), this.GetManagerTypes(), 1);

            if (target == null ||
                target.Count == 0) return null;
            return target[0];
        }

        public void SetAttackStatus(bool attackStatus)
        {
            this.canAttack = attackStatus;

            if (this.canAttack)
            {
                //this.heroSkillActivator.CastSkill(EntitySkillName.ArcherNormalAttack);
            }

            //this.timer = this.canAttack ? this.Model.GetStat<float>(StatEnum.AttackSpeed) : 0;
            if (!attackStatus) this.View.skeletonAnimation.SetAnimation("idle");
        }

        public void CastSkill(string skillId, ITargetable target) { }

        public virtual Type[]   GetManagerTypes() { return new[] { typeof(EnemyManager), typeof(CastleManager) }; }
        public virtual string[] GetTags()         { return new[] { "Fly", "Ground", "Boss", "Building" }; }

        protected override UniTask<GameObject> CreateView() { return this.ObjectPoolManager.Spawn(this.Model.AddressableName); }

        public override void Dispose() { this.ObjectPoolManager.Recycle(this.View); }
    }
}