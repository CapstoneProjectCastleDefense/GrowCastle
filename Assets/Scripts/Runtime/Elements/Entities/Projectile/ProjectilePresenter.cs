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

        public Tween FlyToTarget(ITargetable target)
        {
            var id               = this.Model.Id;
            var projectileRecord = this.projectileBlueprint[id];
            var tween = this.View.transform.Fly(this.Model.StartPoint,
                this.Model.EndPoint,
                projectileRecord.Fragment,
                projectileRecord.ProjectileSpeed,
                projectileRecord.Delay,
                projectileRecord.VectorOrientation);

            tween.onComplete += () =>
            {
                this.View.Recycle();
                DOTween.Kill(this.View.transform);
                target.OnGetHit(this.Model.Damage);
            };

            return tween;
        }

        public override void Dispose() { }
    }
}