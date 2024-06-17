namespace Runtime.Elements.Entities.Projectile
{
    using Cysharp.Threading.Tasks;
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

        protected override UniTask<GameObject> CreateView() { return this.ObjectPoolManager.Spawn(this.Model.AddressableName); }

        public void FlyToTarget()
        {
            var id               = this.Model.Id;
            var projectileRecord = this.projectileBlueprint[id];
            this.View.transform.Fly(this.Model.StartPoint,
                                    this.Model.EndPoint,
                                    projectileRecord.Fragment,
                                    projectileRecord.ProjectileSpeed,
                                    projectileRecord.Delay,
                                    new Vector3(1, 1, -7),
                                    null);
        }

        public override void Dispose() {   }
    }
}