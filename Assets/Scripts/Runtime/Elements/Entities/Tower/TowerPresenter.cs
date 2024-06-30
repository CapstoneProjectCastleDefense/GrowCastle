namespace Runtime.Elements.Entities.Tower
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.EntitySkills;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Systems;
    using UnityEngine;

    public class TowerPresenter: BaseElementPresenter<TowerModel, TowerView, TowerPresenter>, ITowerPresenter
    {
        private readonly EntitySkillSystem entitySkillSystem;
        private readonly TowerBlueprint towerBlueprint;
        private readonly FindTargetSystem findTargetSystem;

        private bool canAttack;
        private float timer;

        protected TowerPresenter(TowerModel model, ObjectPoolManager objectPoolManager, EntitySkillSystem entitySkillSystem, TowerBlueprint towerBlueprint, FindTargetSystem findTargetSystem) : base(model, objectPoolManager) {
            this.entitySkillSystem = entitySkillSystem;
            this.towerBlueprint = towerBlueprint;
            this.findTargetSystem = findTargetSystem;
        }

        public override void Tick() {
            if (!this.canAttack) return;
            if (this.timer >= 1 / this.Model.GetStat<float>(StatEnum.AttackSpeed))
            {
                this.Attack(null);
                this.timer = 0;
            }

            this.timer += Time.deltaTime;
        }

        public void Attack(ITargetable target) {
            var towerDataRecord = this.towerBlueprint.GetDataById(this.Model.Id);

            target ??= this.FindTarget();

            if (target == null) return;
            var enemy = (IElementPresenter)target;

            var skillId = towerDataRecord.SkillToAnimationRecords.ElementAt(0).Key;
            this.entitySkillSystem.CastSkill(skillId, new ProjectileSkillModel()
            {
                Id = skillId,
                StartPoint = this.View.spawnProjectilePos.position,
                EndPoint = enemy.GetView().transform.position,
                Target = target,
                damage = this.Model.GetStat<float>(StatEnum.Attack),
            });
        }
        public ITargetable FindTarget() {
            var priority = this.Model.GetStat<AttackPriorityEnum>(StatEnum.AttackPriority);

            var res = this.findTargetSystem.GetTarget(this, priority, new()
                {
                    AttackPriorityEnum.Ground.ToString(),
                    AttackPriorityEnum.Fly.ToString(),
                    AttackPriorityEnum.Boss.ToString(),
                },
                this.GetManagerTypes());

            return res;
        }
        public float AttackCooldownTime { get; }
        public Type[] GetManagerTypes() { return new[] { typeof(Managers.EnemyManager), typeof(Managers.CastleManager) }; }

        public void CastSkill(string skillId, ITargetable target) {
            
        }

        public void SetAttackStatus(bool attackStatus) {
            this.canAttack = attackStatus;
            this.timer = this.canAttack ? this.Model.GetStat<float>(StatEnum.AttackSpeed) : 0;
        }

        protected override UniTask<GameObject> CreateView() {
            return this.ObjectPoolManager.Spawn(this.towerBlueprint.GetDataById(this.Model.Id).PrefabName);
        }

        public override async UniTask UpdateView() {
            await base.UpdateView();
            Transform transform;
            (transform = this.View.transform).SetParent(this.Model.ParentView);
            transform.localPosition = Vector3.zero;
        }

        public override void Dispose() {
        }
    }
}