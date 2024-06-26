namespace Runtime.Elements.Entities.Hero
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Elements.EntitySkills;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Interfaces.Skills;
    using Runtime.Systems;
    using UnityEngine;

    public class HeroPresenter : BaseElementPresenter<HeroModel, HeroView, HeroPresenter>, IHeroPresenter
    {
        private readonly EntitySkillSystem entitySkillSystem;
        private readonly HeroBlueprint     heroBlueprint;
        private readonly FindTargetSystem  findTargetSystem;

        private bool  canAttack;
        private float timer;

        protected HeroPresenter(HeroModel model, ObjectPoolManager objectPoolManager, EntitySkillSystem entitySkillSystem, HeroBlueprint heroBlueprint, FindTargetSystem findTargetSystem) : base(model, objectPoolManager)
        {
            this.entitySkillSystem = entitySkillSystem;
            this.heroBlueprint     = heroBlueprint;
            this.findTargetSystem  = findTargetSystem;
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

        private void CastSkillInternal(string skillId, ITargetable target, IEntitySkillModel skillModel)
        {
            var heroDataRecord = this.heroBlueprint.GetDataById(this.Model.Id);
            this.View.skeletonAnimation.SetAnimation(heroDataRecord.SkillToAnimationRecords[skillId].AnimationSkillName, loop: false);
            this.entitySkillSystem.CastSkill(skillId, skillModel);
            UniTask.Delay(TimeSpan.FromSeconds(1f)).ContinueWith(() =>
            {
                this.View.skeletonAnimation.SetAnimation("idle", loop: true);
            });
        }

        public void CastSkill(string skillId, ITargetable target)
        {
            this.CastSkillInternal(skillId, target, new BasicSkillModel()
            {
                Id    = skillId,
                Level = 1,
            });
        }

        public Type[] GetManagerTypes() { return new[] { typeof(Managers.CastleManager), typeof(Managers.EnemyManager) }; }

        public void SetAttackStatus(bool attackStatus)
        {
            this.canAttack = attackStatus;
            this.timer     = this.canAttack ? this.Model.GetStat<float>(StatEnum.AttackSpeed) : 0;
        }

        public void OnHeroUpgrade() { }

        public void Attack(ITargetable target)
        {
            var heroDataRecord = this.heroBlueprint.GetDataById(this.Model.Id);

            if (heroDataRecord.Class != HeroClass.Attack) return;

            target ??= this.FindTarget();

            if (target == null) return;
            var enemy = (IElementPresenter)target;

            var skillId = heroDataRecord.SkillToAnimationRecords.ElementAt(1).Key;
            this.CastSkillInternal(skillId, target, new ProjectileSkillModel()
            {
                Id         = skillId,
                StartPoint = this.View.spawnProjectilePos.position,
                EndPoint   = enemy.GetView().transform.position,
                Target     = target,
                damage     = this.Model.GetStat<float>(StatEnum.Attack),
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
                },
                this.GetManagerTypes());

            return res;
        }

        public float AttackCooldownTime { get; }

        public void Equip(IEquipment equipment) { }

        public void UnEquip(IEquipment equipment) { }

        protected override UniTask<GameObject> CreateView() { return this.ObjectPoolManager.Spawn(this.heroBlueprint.GetDataById(this.Model.Id).PrefabName); }

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            Transform transform;
            (transform = this.View.transform).SetParent(this.Model.ParentView);
            transform.localPosition = Vector3.zero;
            var listSkill = this.heroBlueprint.GetDataById(this.Model.Id).SkillToAnimationRecords;
            this.View.OnClickAction = () => this.CastSkill(listSkill.First().Key, null);
        }

        public override void Dispose() { }
    }
}