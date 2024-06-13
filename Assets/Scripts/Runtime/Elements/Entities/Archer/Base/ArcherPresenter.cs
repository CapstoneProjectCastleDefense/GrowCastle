namespace Runtime.Elements.Entities.Archer.Base
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class ArcherPresenter : BaseElementPresenter<ArcherModel, ArcherView, ArcherPresenter>, IArcherPresenter
    {
        protected ArcherPresenter(ArcherModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            this.View.transform.SetParent(this.Model.ParentView);
            this.View.transform.localPosition                   = Vector3.zero;
            this.View.GetComponent<MeshRenderer>().sortingOrder = this.Model.Index;
        }

        public void Attack(ITargetable target = null)
        {
            if (target == null)
                target = this.FindTarget();
            if (target == null) return;
            var attackPower = this.Model.GetStat<float>(StatEnum.Attack);
        }
        public ITargetable FindTarget()
        {
            var attackPriority = this.Model.GetStat<AttackPriorityEnum>(StatEnum.AttackPriority);
            return null;
        }

        public void CastSkill(IEntitySkillPresenter entitySkillPresenter, ITargetable target)
        {

        }

        protected override UniTask<GameObject> CreateView()
        {
            return this.ObjectPoolManager.Spawn(this.Model.AddressableName);
        }

        public override void Dispose()
        {
            this.ObjectPoolManager.Recycle(this.View);
        }
    }
}