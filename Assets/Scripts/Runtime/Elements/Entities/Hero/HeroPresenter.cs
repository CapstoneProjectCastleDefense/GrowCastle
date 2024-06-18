namespace Runtime.Elements.Entities.Hero
{
    using System;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Interfaces.Skills;
    using Runtime.Systems;
    using UnityEngine;

    public class HeroPresenter : BaseElementPresenter<HeroModel, HeroView, HeroPresenter>, IHeroPresenter
    {
        private readonly EntitySkillSystem entitySkillSystem;
        private readonly HeroBlueprint     heroBlueprint;

        protected HeroPresenter(HeroModel model, ObjectPoolManager objectPoolManager, EntitySkillSystem entitySkillSystem, HeroBlueprint heroBlueprint) : base(model, objectPoolManager)
        {
            this.entitySkillSystem = entitySkillSystem;
            this.heroBlueprint     = heroBlueprint;
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

        public void Attack(ITargetable target) { }

        public ITargetable FindTarget() { return null; }

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