namespace Runtime.Elements.Entities.Hero
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.EntitySkills;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
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
            this.View.skeletonAnimation.SetAnimation("summon", loop: false);
            this.entitySkillSystem.CastSkill(skillId, new SummonSkillModel
            {
                Number        = 2,
                PrefabName    = "SummonKnight",
                StartPos      = new Vector3(-6.51f, -1.38f, 0),
                DistanceRange = 1f
            });
            //this.View.skeletonAnimation.SetAnimation("idle",loop: true);
        }

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
            var listSkill = this.heroBlueprint.GetDataById(this.Model.Id).Skill;
            this.View.OnClickAction = () => this.CastSkill(listSkill.First(), null);
        }

        public override void Dispose() { }
    }
}