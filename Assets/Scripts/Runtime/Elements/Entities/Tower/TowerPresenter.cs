namespace Runtime.Elements.Entities.Tower
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.EntitySkills;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Systems;
    using UnityEngine;

    public class TowerPresenter : BaseCombatantPresenter<TowerModel, TowerView, TowerPresenter>, ITowerPresenter
    {
        private readonly EntitySkillSystem entitySkillSystem;
        private readonly FindTargetSystem  findTargetSystem;
        private readonly HeroBlueprint     heroBlueprint;

        private bool  canAttack;
        private float timer;

        protected TowerPresenter(
            TowerModel model,
            ObjectPoolManager objectPoolManager,
            EntitySkillSystem entitySkillSystem,
            FindTargetSystem findTargetSystem,
            HeroBlueprint heroBlueprint)
            : base(model, objectPoolManager)
        {
            this.entitySkillSystem = entitySkillSystem;
            this.findTargetSystem  = findTargetSystem;
            this.heroBlueprint     = heroBlueprint;
        }

        public override void Tick()
        {
            if (!this.canAttack) return;
            if (this.timer >= 1 / this.Model.GetStat<float>(StatEnum.AttackSpeed))
            {
                this.Attack(null);
                this.timer = 0;
            }

            this.timer += Time.deltaTime;
        }

        public void Attack(ITargetable target)
        {
            var towerDataRecord = this.heroBlueprint.GetDataById(this.Model.Id);

            target ??= this.FindTarget();

            if (target == null) return;

            var attackSkill = towerDataRecord.AttackSkill;
            this.CastSkillInternal(attackSkill.skillName, attackSkill.animationName, target, new BaseProjectileSkillModel()
            {
                Id         = attackSkill.skillName,
                StartPoint = this.View.spawnProjectilePos.position,
                EndPoint   = target.GetGameObject().transform.position,
                Target     = target,
                Damage     = this.Model.GetStat<float>(StatEnum.Attack),
                Caster     = this,
            });
        }
        public ITargetable FindTarget()
        {
            var priority = this.Model.GetStat<AttackPriorityEnum>(StatEnum.AttackPriority);

            var res = this.findTargetSystem.GetTarget(this, priority, this.GetTags().ToList(), this.GetManagerTypes(), 2);

            return res.Count > 0 ? res.RandomElement() : null;
        }
        public         float    AttackCooldownTime { get; }
        public virtual Type[]   GetManagerTypes()  { return new[] { typeof(CastleManager), typeof(EnemyManager) }; }
        public virtual string[] GetTags()          { return new[] { "Fly", "Ground", "Boss", }; }

        public void CastSkill(string skillId, string animationName, ITargetable target) { }

        private void CastSkillInternal(string skillId, string animationName, ITargetable target, IEntitySkillModel skillModel)
        {
            var heroDataRecord = this.heroBlueprint.GetDataById(this.Model.Id);
            this.View.skeletonAnimation.SetAnimation(animationName, loop: false);
            this.entitySkillSystem.CastSkill(skillId, skillModel);
            UniTask.Delay(TimeSpan.FromSeconds(1f)).ContinueWith(() => { this.View.skeletonAnimation.SetAnimation("idle", loop: true); });
        }

        public void SetAttackStatus(bool attackStatus)
        {
            this.canAttack = attackStatus;
            this.timer     = this.canAttack ? this.Model.GetStat<float>(StatEnum.AttackSpeed) : 0;
        }

        protected override UniTask<GameObject> CreateView() { return this.ObjectPoolManager.Spawn(this.heroBlueprint.GetDataById(this.Model.Id).PrefabName); }

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            Transform transform;
            (transform = this.View.transform).SetParent(this.Model.ParentView);
            transform.localPosition = Vector3.zero;
        }

        public override void Dispose()
        {
            if (this.View != null) this.View.Recycle();
            this.ElementManager.entities.Remove(this);
        }
    }
}