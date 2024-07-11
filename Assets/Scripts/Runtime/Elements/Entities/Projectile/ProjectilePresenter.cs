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
        private Tween flyTween;

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

        protected override UniTask<GameObject> CreateView() { return this.ObjectPoolManager.Spawn(this.projectileBlueprint[this.Model.Id].PrefabName); }

        protected override UniTask InitView()
        {
            this.View.projectileHitTrigger += this.OnProjectileHit;
            return base.InitView();
        }

        public Tween FlyToTarget()
        {
            var id               = this.Model.Id;
            var projectileRecord = this.projectileBlueprint[id];
            this.flyTween = this.View.transform.Fly(this.Model.StartPoint,
                this.Model.EndPoint,
                projectileRecord.Fragment,
                projectileRecord.ProjectileSpeed,
                projectileRecord.Delay,
                projectileRecord.VectorOrientation);

            this.flyTween.onComplete += () =>
            {
                this.View.Recycle();
                DOTween.Kill(this.View.transform);
            };

            return this.flyTween;
        }

        private void OnProjectileHit(Collider2D collider2D)
        {
            if (collider2D.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                this.View.projectileHitTrigger -= this.OnProjectileHit;
                this.Model.OnProjectileHit?.Invoke(collider2D);
                this.flyTween?.Kill();
                this.View.Recycle();
            }
        }

        public override void Dispose() { }
    }
}