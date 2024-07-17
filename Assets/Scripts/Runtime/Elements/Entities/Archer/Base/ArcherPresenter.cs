namespace Runtime.Elements.Entities.Archer.Base
{
    using System;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.Extension;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Castles.ArcherSlots;
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
        private readonly HeroSkillActivator heroSkillActivator;
        private          bool              canAttack;

        protected ArcherPresenter(
            ArcherModel model,
            ObjectPoolManager objectPoolManager,
            EnemyManager enemyManager,
            FindTargetSystem findTargetSystem,
            HeroSkillActivator heroSkillActivator
        )
            : base(model, objectPoolManager)
        {
            this.enemyManager      = enemyManager;
            this.findTargetSystem  = findTargetSystem;
            this.heroSkillActivator = heroSkillActivator;
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

        public override void Tick() { }

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