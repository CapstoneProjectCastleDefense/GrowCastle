namespace Runtime.Elements.Entities.Projectile
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using UnityEngine;

    public class ProjectilePresenter : BaseElementPresenter<ProjectileModel, ProjectileView, ProjectilePresenter>
    {
        private readonly ProjectileBlueprint projectileBlueprint;

        public ProjectilePresenter(
            ProjectileModel model,
            ObjectPoolManager objectPoolManager,
            ProjectileBlueprint projectileBlueprint)
            : base(model, objectPoolManager)
        {
            this.projectileBlueprint = projectileBlueprint;
        }

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            this.View.transform.position = this.Model.StartPoint;
        }

        protected override UniTask<GameObject> CreateView()
        {
            var projectileRecord = this.projectileBlueprint[this.Model.Id];
            return this.ObjectPoolManager.Spawn(projectileRecord.PrefabName);
        }

        public void FlyToTarget(ITargetable target)
        {
            var id               = this.Model.Id;
            var projectileRecord = this.projectileBlueprint[id];
            this.View.transform.Fly(this.Model.StartPoint,
                                    this.Model.EndPoint,
                                    projectileRecord.Fragment,
                                    projectileRecord.ProjectileSpeed,
                                    projectileRecord.Delay,
                                    projectileRecord.VectorOrientation)
                .onComplete += () =>
            {
                this.View.Recycle();
                DOTween.Kill(this.View.transform);
                target.OnGetHit(this.Model.Damage);
            };
        }

        public override void Dispose() { }
    }
}