namespace Runtime.Elements.Entities.Enemy
{
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using Runtime.Enums;
    using Runtime.Interfaces;
    using Runtime.Interfaces.Entities;
    using UnityEngine;

    public class EnemyPresenter : BaseElementPresenter<EnemyModel, EnemyView, EnemyPresenter>, IEnemyPresenter
    {
        protected EnemyPresenter(EnemyModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager) { }
        public void Attack(ITargetable target)
        {
            this.View.Animator.SetTrigger("Attack");
            target.OnGetHit(this.Model.GetStat<float>(StatEnum.Attack));
        }
        public ITargetable FindTarget() { return null; }
        public void OnGetHit(float damage)
        {
            var currentHealth = this.Model.GetStat<float>(StatEnum.Health);
            currentHealth -= damage;
            this.Model.SetStat(StatEnum.Health, currentHealth);
        }
        

        public void OnDeath()
        {
            this.View.Animator.SetTrigger("Death");
            var clip = this.View.Animator.runtimeAnimatorController.animationClips.First(x => x.name == "Death");
            UniTask.Delay((int)(clip.length * 1000)).ContinueWith(this.Dispose);
            this.Dispose();
        }
        protected override UniTask<GameObject> CreateView()
        {
            var res = this.ObjectPoolManager.Spawn(this.Model.AddressableName);
            return res;
        }
        public override void Dispose() { this.ObjectPoolManager.Recycle(this.View); }
    }
}