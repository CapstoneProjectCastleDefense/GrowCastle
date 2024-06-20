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
            this.Attack(null);
        }

        public void CastSkill(string skillId, ITargetable target)
        {
            var heroDataRecord = this.heroBlueprint.GetDataById(this.Model.Id);
            this.View.skeletonAnimation.SetAnimation(heroDataRecord.SkillToAnimationRecords[skillId].AnimationSkillName, loop: false);
            this.entitySkillSystem.CastSkill(skillId, new BasicSkillModel()
            {
                Id    = skillId,
                Level = 1,
            });
            UniTask.Delay(TimeSpan.FromSeconds(1f)).ContinueWith(() =>
            {
                this.View.skeletonAnimation.SetAnimation("idle", loop: true);
            });
        }

        public void OnHeroUpgrade() { }

        public void Attack(ITargetable target)
        {
            var heroDataRecord = this.heroBlueprint.GetDataById(this.Model.Id);
            if (heroDataRecord.Class != HeroClass.Attack) return;

            target ??= this.FindTarget();
            if (target == null) return;
            var enemy = (EnemyPresenter)target;

            var skillId = heroDataRecord.SkillToAnimationRecords.ElementAt(1).Key;
            this.entitySkillSystem.CastSkill(skillId, new ProjectileSkillModel()
            {
                Id               = skillId,
                StartPoint       = this.View.spawnProjectilePos.position,
                EndPoint         = enemy.GetEnemyView.transform.position,
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

            var res = this.findTargetSystem.GetTarget(this, priority, new()
            {
                AttackPriorityEnum.Ground.ToString(),
                AttackPriorityEnum.Fly.ToString(),
                AttackPriorityEnum.Boss.ToString(),
                AttackPriorityEnum.Building.ToString()
            });

            return res;
        }

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