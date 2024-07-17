namespace Runtime.Combat.CombatActions.Actions
{
    using Cysharp.Threading.Tasks;
    using DG.Tweening;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using global::Models.Blueprints;
    using global::Models.Tags;
    using Runtime.Combat.CombatActions.Models;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using UnityEngine;

    public class FireProjectile : ICombatAction
    {
        private readonly ProjectileBlueprint projectileBlueprint;
        private readonly ProjectileManager   projectileManager;
        private readonly EffectManager       effectManager;
        public           string              ActionId { get; set; } = CombatActionId.FireProjectile;

        public FireProjectile(ProjectileBlueprint projectileBlueprint,
                              ProjectileManager projectileManager,
                              EffectManager effectManager)
        {
            this.projectileBlueprint = projectileBlueprint;
            this.projectileManager   = projectileManager;
            this.effectManager       = effectManager;
        }

        public void Execute(ICombatActionModel model)
        {
            var fireProjectileModel = (FireProjectileModel)model;
            var projectileRecord = this.projectileBlueprint.GetDataById(fireProjectileModel.ProjectileId);
            var damage           = fireProjectileModel.CombatantPresenter.GetStats().GetStat<float>(StatEnum.Attack); //todo: calculate damage with crit chance, ...
            this.Fire(new ProjectileModel
                {
                    Id              = projectileRecord.Id,
                    AddressableName = projectileRecord.PrefabName,
                    StartPoint      = fireProjectileModel.CombatantPresenter.GetView().transform.position,
                    EndPoint        = fireProjectileModel.Target.GetGameObject().transform.position,
                    Damage          = damage,
                    Target          = fireProjectileModel.Target,
                    EffectTags      = fireProjectileModel.EffectTags,
                    OnProjectileHit = this.OnProjectileHit
                })
                .Forget();
        }

        protected virtual void OnProjectileHit(Collider2D collider2D, ProjectilePresenter projectile)
        {
            var objHit = collider2D.gameObject;
            if (objHit.layer == projectile.Model.Target.GetGameObject().layer)
            {
                var targetableView = objHit.GetComponentInParent<ITargetableView>();
                if (targetableView != null &&
                    !targetableView.GetTargetablePresenter().IsDead)
                {
                    foreach (var effectTag in projectile.Model.EffectTags)
                    {
                        this.effectManager.Execute(targetableView.GetTargetablePresenter(), effectTag);
                    }

                    projectile.GetView().transform.DOKill();
                    projectile.GetView().Recycle();
                    projectile.isFlyComplete = true;
                }
            }

            projectile.Dispose();
        }

        protected virtual void OnFlyToTarget(ProjectilePresenter projectile) { projectile.Dispose(); }

        private async UniTaskVoid Fire(ProjectileModel projectileModel)
        {
            var projectile = this.projectileManager.CreateElement(projectileModel);

            await projectile.UpdateView();
            projectile.FlyToTarget().onComplete += () => this.OnFlyToTarget(projectile);
        }
    }
}