namespace Runtime.Elements.Entities.Hero
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Managers;
    using Runtime.Systems;
    using UnityEngine;

    public class HeroPresenter : BaseCombatantPresenter<HeroModel, HeroView, HeroPresenter>, IHeroPresenter
    {
        private readonly HeroSkillActivator heroSkillActivator;
        private readonly HeroBlueprint      heroBlueprint;
        private readonly FindTargetSystem   findTargetSystem;
        private readonly SkillBlueprint     skillBlueprint;
        private readonly CastleManager      castleManager;

        private HeroManager heroManager;
        private bool        canAttack;
        public  Action      OnClickAction;

        protected HeroPresenter(
            HeroModel model,
            ObjectPoolManager objectPoolManager,
            HeroSkillActivator heroSkillActivator,
            HeroBlueprint heroBlueprint,
            FindTargetSystem findTargetSystem,
            SkillBlueprint skillBlueprint,
            CastleManager castleManager)
            : base(model, objectPoolManager)
        {
            this.heroSkillActivator = heroSkillActivator;
            this.heroBlueprint      = heroBlueprint;
            this.findTargetSystem   = findTargetSystem;
            this.skillBlueprint     = skillBlueprint;
            this.castleManager      = castleManager;
        }

        public void SetManager(HeroManager heroManager) => this.heroManager = heroManager;

        public override void Tick() { }

        // private void CastSkillInternal(string skillId)
        // {
        //     var heroDataRecord = this.heroBlueprint.GetDataById(this.Model.Id);
        //     this.View.skeletonAnimation.SetAnimation(heroDataRecord.SkillToAnimationRecords[skillId].AnimationSkillName, loop: false);
        //     UniTask.Delay(TimeSpan.FromSeconds(1f))
        //            .ContinueWith(() =>
        //            {
        //                this.View.skeletonAnimation.SetAnimation("idle", loop: true);
        //            })
        //            .Forget();
        // }

        public virtual void CastSkill(string skillId, ITargetable target)
        {
            // if (this.View.cooldownSkillBar.fillAmount < 1) return;
            // if (!this.castleManager.UseManaForSkill(this.skillBlueprint.GetDataById(skillId).Mana)) return;
            // this.CastSkillInternal(skillId);
            // this.View.cooldownSkillBar.fillAmount = 0;
            // this.StartRefillCooldown(this.skillBlueprint.GetDataById(skillId).Cooldown);
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

        public virtual Type[]   GetManagerTypes() { return new[] { typeof(CastleManager), typeof(EnemyManager) }; }
        public virtual string[] GetTags()         { return new[] { "Fly", "Ground", "Boss", }; }

        public void SetAttackStatus(bool attackStatus)
        {
            this.canAttack = attackStatus;
            if (attackStatus)
            {
                foreach (var skillId in this.Model.Skills)
                {
                    this.heroSkillActivator.Activate(skillId, this);
                }
            }
            else
            {
                foreach (var skillId in this.Model.Skills)
                {
                    this.heroSkillActivator.Deactivate(skillId, this);
                }
            }
        }

        public void Attack(ITargetable target)
        {
            // var heroDataRecord = this.heroBlueprint.GetDataById(this.Model.Id);
            //
            // if (heroDataRecord.Class != HeroClass.Attack) return;
            //
            // target ??= this.FindTarget();
            //
            // if (target == null) return;
            //
            // var skillId = heroDataRecord.SkillToAnimationRecords.ElementAt(1).Key;
            // this.CastSkillInternal(skillId);
        }

        public ITargetable FindTarget()
        {
            var priority = this.Model.GetStat<AttackPriorityEnum>(StatEnum.AttackPriority);

            var res = this.findTargetSystem.GetTarget(this, priority, this.GetTags().ToList(), this.GetManagerTypes(), 2);

            return res.Count > 0 ? res.RandomElement() : null;
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
            // var listSkill = this.heroBlueprint.GetDataById(this.Model.Id).SkillToAnimationRecords;
            this.View.onClickAction = () =>
            {
                this.OnClickAction?.Invoke();
            };
        }

        public override void Dispose()
        {
            if (this.View != null) this.View.Recycle();
            this.heroManager.entities.Remove(this);
        }
    }
}