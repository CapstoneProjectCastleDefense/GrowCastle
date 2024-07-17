namespace Runtime.Elements.EntitySkills
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.Systems;
    using UnityEngine;
    using Zenject;

    public abstract class BaseHeroProjectileSkill<TModel> : BaseHeroSkill
        where TModel : BaseProjectileHeroSkillModel
    {
        protected readonly ProjectileManager   projectileManager;
        protected readonly ProjectileBlueprint projectileBlueprint;
        
        protected BaseHeroProjectileSkill(SignalBus signalBus,
                                      FindTargetSystem findTargetSystem, 
                                      EffectManager effectManager, 
                                      VFXService vfxService,
                                      ProjectileManager projectileManager,
                                      ProjectileBlueprint projectileBlueprint) 
            : base(signalBus, findTargetSystem, effectManager, vfxService)
        {
            this.projectileManager   = projectileManager;
            this.projectileBlueprint = projectileBlueprint;
        }

        protected readonly List<ProjectilePresenter> firedProjectiles = new();

        protected async UniTaskVoid FireProjectile(ProjectileModel projectileModel)
        {
            var projectile = this.projectileManager.CreateElement(projectileModel);

            await projectile.UpdateView();
            this.firedProjectiles.Add(projectile);
            projectile.FlyToTarget().onComplete += () => this.OnFlyToTarget(projectile);
        }

        protected virtual void OnFlyToTarget(ProjectilePresenter projectile) { this.RemoveProjectile(projectile); }

        protected virtual void OnProjectileHit(Collider2D collider2D, ProjectilePresenter projectile) { }

        protected void RemoveProjectile(ProjectilePresenter projectile)
        {
            if (this.firedProjectiles.Contains(projectile))
            {
                this.firedProjectiles.Remove(projectile);
            }
        }
    }

    public class BaseProjectileHeroSkillModel : IHeroSkillModel
    {
        public string      Id              { get; set; }
        public string      AddressableName { get; set; }
        public string      Description     { get; }
        public string      Name            { get; }
        public Vector3     StartPoint;
        public Vector3     EndPoint;
        public ITargetable Target;
        public float       Damage;
    }
}