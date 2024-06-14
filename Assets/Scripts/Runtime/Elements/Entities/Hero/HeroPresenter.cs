namespace Runtime.Elements.Entities.Hero
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Items;
    using Runtime.Systems;
    using UnityEngine;

    public class HeroPresenter : BaseElementPresenter<HeroModel, HeroView, HeroPresenter>, IHeroPresenter
    {
        private readonly SkillSystem   skillSystem;
        private readonly HeroBlueprint heroBlueprint;

        protected HeroPresenter(HeroModel model, ObjectPoolManager objectPoolManager, SkillSystem skillSystem, HeroBlueprint heroBlueprint) : base(model, objectPoolManager)
        {
            this.skillSystem   = skillSystem;
            this.heroBlueprint = heroBlueprint;
        }


        public void CastSkill(string skillId, ITargetable target)
        {
            this.skillSystem.CastSkill(skillId,new SummonKnightSkillModel());
        }

        public void Attack(ITargetable target)
        {
        }

        public ITargetable FindTarget()
        {
            return null;
        }

        public void Equip(IEquipment equipment)
        {
        }

        public void Unequip(IEquipment equipment)
        {
        }

        protected override UniTask<GameObject> CreateView()
        {
            return this.ObjectPoolManager.Spawn(this.heroBlueprint.GetDataById(this.Model.Id).PrefabName);
        }

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            this.View.transform.SetParent(this.Model.ParentView);
            this.View.transform.localPosition = Vector3.zero;
        }

        public override void Dispose()
        {
        }

    }
}