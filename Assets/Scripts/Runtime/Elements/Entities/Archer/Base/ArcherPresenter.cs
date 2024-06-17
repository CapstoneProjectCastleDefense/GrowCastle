namespace Runtime.Elements.Entities.Archer.Base
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Enemy;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using UnityEngine;

    public class ArcherPresenter : BaseElementPresenter<ArcherModel, ArcherView, ArcherPresenter>, IArcherPresenter
    {
        private readonly EnemyManager enemyManager;
        private          bool         canAttack;
        private          float        timer;
        protected ArcherPresenter(ArcherModel model, ObjectPoolManager objectPoolManager,EnemyManager enemyManager) : base(model, objectPoolManager) { this.enemyManager = enemyManager; }

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            Transform transform;
            (transform = this.View.transform).SetParent(this.Model.ParentView);
            transform.localPosition                   = Vector3.zero;
            this.View.GetComponent<MeshRenderer>().sortingOrder = this.Model.Index + 1;
        }

        public override void Tick()
        {
            if (!this.canAttack) return;
            if (this.timer >= 1/this.Model.GetStat<float>(StatEnum.AttackSpeed))
            {
                var target = this.enemyManager.entities.Count > 0 ? this.enemyManager.entities.First() : null;
                this.Attack(target);
                this.timer = 0;
            }

            this.timer += Time.deltaTime;
        }

        public void SetAttackStatus(bool attackStatus)
        {
            this.canAttack = attackStatus;
            this.timer     = this.canAttack ? this.Model.GetStat<float>(StatEnum.AttackSpeed) : 0;
        }

        public void Attack(ITargetable target = null)
        {
            target ??= this.FindTarget();
            if (target == null) return;
            var enemy       = (EnemyPresenter)target;
            
            this.View.skeletonAnimation.SetAnimation("attack",false);
            var attackPower = this.Model.GetStat<float>(StatEnum.Attack);
            
            var arrow = this.ObjectPoolManager.Spawn(this.View.arrowPrefab, this.View.spawnArrowPos);
            arrow.transform.localPosition = Vector3.zero;
            arrow.transform.Fly(arrow.transform.position, enemy.GetEnemyView.transform.position, 3, 0.7f, 0, new Vector3(1,1,-7), () =>
            {
                arrow.Recycle();
                DOTween.Kill(arrow.transform);
                if(!enemy.IsDead) enemy.OnGetHit(attackPower);
            });
            
        }

        public ITargetable FindTarget()
        {
            //var attackPriority = this.Model.GetStat<AttackPriorityEnum>(StatEnum.AttackPriority);

            return null;
        }

        public void CastSkill(string skillId, ITargetable target)
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