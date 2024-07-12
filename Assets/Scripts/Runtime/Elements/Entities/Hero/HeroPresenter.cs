namespace Runtime.Elements.Entities.Hero
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.EntitySkills;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;

    public class HeroPresenter : BaseCombatantPresenter<HeroModel, HeroView, HeroPresenter>, IHeroPresenter
    {
        private readonly EntitySkillSystem entitySkillSystem;
        private readonly HeroBlueprint     heroBlueprint;
        private readonly FindTargetSystem  findTargetSystem;
        private readonly SkillBlueprint    skillBlueprint;
        private readonly CastleManager     castleManager;

        private HeroManager                  heroManager;
        private bool                         canAttack;
        private float                        timer;
        public  int                          AttackCount;
        public  List<IPassiveSkillPresenter> PassiveSkillPresenters = new();

        protected HeroPresenter(
            HeroModel model,
            ObjectPoolManager objectPoolManager,
            EntitySkillSystem entitySkillSystem,
            HeroBlueprint heroBlueprint,
            FindTargetSystem findTargetSystem,
            SkillBlueprint skillBlueprint,
            CastleManager castleManager)
            : base(model, objectPoolManager)
        {
            this.entitySkillSystem = entitySkillSystem;
            this.heroBlueprint     = heroBlueprint;
            this.findTargetSystem  = findTargetSystem;
            this.skillBlueprint    = skillBlueprint;
            this.castleManager     = castleManager;
        }

        public void SetManager(HeroManager heroManager) => this.heroManager = heroManager;

        public override void Tick()
        {
            if (!this.canAttack) return;
            if (this.timer >= 1 / this.Model.GetStat<float>(StatEnum.AttackSpeed))
            {
                this.Attack(null);
                this.timer = 0;
                this.AttackCount++;
            }

            this.PassiveSkillPresenters.ForEach(e => e.Tick());

            this.timer += Time.deltaTime;
        }

        private void CastSkillInternal(string skillId, string animationName, ITargetable target, IEntitySkillModel skillModel)
        {
            this.View.skeletonAnimation.SetAnimation(animationName, loop: false);
            this.entitySkillSystem.CastSkill(skillId, skillModel);
            UniTask.Delay(TimeSpan.FromSeconds(1f)).ContinueWith(() => { this.View.skeletonAnimation.SetAnimation("idle", loop: true); });
        }

        public void CastSkill(string skillId, string animationName, ITargetable target)
        {
            if (this.View.cooldownSkillBar.fillAmount < 1) return;
            if (!this.castleManager.UseManaForSkill(this.skillBlueprint.GetDataById(skillId).Mana - this.Model.GetStat<float>(StatEnum.BonusReduceMana))) return;
            this.CastSkillInternal(skillId, animationName, target, new BasicSkillModel()
            {
                Id    = skillId,
                Level = 1,
            });
            this.View.cooldownSkillBar.fillAmount = 0;
            this.StartRefillCooldown(this.Model.GetStat<float>(StatEnum.ActiveSkillCooldown));
        }

        private void StartRefillCooldown(float cooldownTime)
        {
            DOTween.Kill(this.View.cooldownSkillBar);
            this.View.cooldownSkillBar.DOFillAmount(1, cooldownTime).SetEase(Ease.Linear);
        }
        public void ResetCooldown()
        {
            DOTween.Kill(this.View.cooldownSkillBar);
            this.View.cooldownSkillBar.fillAmount = 1;
        }

        public void SetRaycastActive(bool isActive)
        {
            if (this.View == null) return;
            this.View.GetComponent<BoxCollider2D>().enabled = isActive;
        }

        public virtual Type[]   GetManagerTypes() { return new[] { typeof(CastleManager), typeof(EnemyManager) }; }
        public virtual string[] GetTags()         { return new[] { "Fly", "Ground", "Boss", }; }

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

            var skillId = heroDataRecord.AttackSkill.skillName;
            this.CastSkillInternal(skillId, heroDataRecord.AttackSkill.animationName, target, new BaseProjectileSkillModel()
            {
                Id         = skillId,
                StartPoint = this.View.spawnProjectilePos.position,
                EndPoint   = target.GetGameObject().transform.position,
                Target     = target,
                Damage     = this.Model.GetStat<float>(StatEnum.Attack),
            });
        }

        public ITargetable FindTarget()
        {
            var priority = this.Model.GetStat<AttackPriorityEnum>(StatEnum.AttackPriority);

            var res = this.findTargetSystem.GetTarget(this, priority, this.GetTags().ToList(), this.GetManagerTypes(), 2);

            return res.Count > 0 ? res.RandomElement() : null;
        }

        public float AttackCooldownTime { get; }

        #region Implement IEquipable

        public Dictionary<EquipmentType, IEquipment> Equipment { get; } = new();

        public void Equip(IEquipment equipment) { equipment.OnEquip(this.Model); }

        public void UnEquip(IEquipment equipment) { equipment.OnUnEquip(this.Model); }

        #endregion

        #region Implement IElementPresenter

        protected override UniTask<GameObject> CreateView() { return this.ObjectPoolManager.Spawn(this.heroBlueprint.GetDataById(this.Model.Id).PrefabName); }

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            Transform transform;
            (transform = this.View.transform).SetParent(this.Model.ParentView);
            transform.localPosition = Vector3.zero;
            var activeSkill = this.heroBlueprint.GetDataById(this.Model.Id).ActiveSkill;
            this.View.OnClickAction = () => this.CastSkill(activeSkill.skillName, activeSkill.animationName, null);
            this.heroBlueprint.GetDataById(this.Model.Id).PassiveSkill?.ForEach(passiveSkillName => { this.entitySkillSystem.ActivePassiveSkill(passiveSkillName, this); });
        }

        public override void Dispose()
        {
            if (this.View != null) this.View.Recycle();
            this.heroManager.entities.Remove(this);
        }

        #endregion
    }
}