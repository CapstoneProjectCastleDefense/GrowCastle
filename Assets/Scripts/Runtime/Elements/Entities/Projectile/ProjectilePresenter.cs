namespace Runtime.Elements.Entities.Projectile
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Extensions;
    using UnityEngine;

    public class ProjectilePresenter : BaseElementPresenter<ProjectileModel, ProjectileView, ProjectilePresenter>
    {
        private readonly ProjectileBlueprint projectileBlueprint;

        public ProjectilePresenter(ProjectileModel model,
            ObjectPoolManager objectPoolManager,
            ProjectileBlueprint projectileBlueprint)
            : base(model, objectPoolManager)
        {
            this.projectileBlueprint = projectileBlueprint;
        }


        protected override UniTask<GameObject> CreateView()
        {
            var obj = this.ObjectPoolManager.Spawn(this.Model.Prefab); //consider of load view from addressable with id or with prefab
            obj.transform.position = this.Model.StartPoint;
            return new UniTask<GameObject>(obj);
        }

        public void FlyToTarget()
        {
            var id               = this.Model.Id;
            var projectileRecord = this.projectileBlueprint[id];
            this.View.transform.Fly(this.Model.StartPoint,
                this.Model.EndPoint,
                projectileRecord.Fragment,
                projectileRecord.ProjectileSpeed,
                projectileRecord.Delay,
                new Vector3(1, 1, -7)).onComplete += () =>
            {
                this.View.Recycle();
                DOTween.Kill(this.View.transform);
            };
        }

        public override void Dispose() { }
    }
}