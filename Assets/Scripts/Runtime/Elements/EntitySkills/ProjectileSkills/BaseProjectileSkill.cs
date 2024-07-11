namespace Runtime.Elements.EntitySkills
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Models.Tags;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using UnityEngine;

    public abstract class BaseProjectileSkill<TModel> : BaseEntitySkillPresenter<TModel>
        where TModel : BaseProjectileSkillModel
    {
        protected readonly ProjectileManager projectileManager;
        protected readonly AbilitySystem     abilitySystem;
        protected readonly EffectManager     effectManager;

        public BaseProjectileSkill(ProjectileManager projectileManager,
            IGameAssets gameAssets,
            ProjectileBlueprint projectileBlueprint,
            AbilitySystem abilitySystem, EffectManager effectManager)
        {
            this.projectileManager = projectileManager;
            this.abilitySystem     = abilitySystem;
            this.effectManager     = effectManager;
        }

        protected readonly List<ProjectilePresenter> firedProjectiles = new();

        protected override void InternalActivate() { this.FireProjectile().Forget(); }

        private async UniTaskVoid FireProjectile()
        {
            var projectile = this.projectileManager.CreateElement(new()
            {
                Id              = this.Model.Id,
                AddressableName = this.Model.AddressableName,
                StartPoint      = this.Model.StartPoint,
                EndPoint        = this.Model.EndPoint,
                Damage          = this.Model.Damage,
                OnProjectileHit = this.OnProjectileHit
            });

            await projectile.UpdateView();
            this.firedProjectiles.Add(projectile);
            projectile.FlyToTarget().onComplete += () => this.OnFlyToTarget(projectile);
        }

        protected virtual void OnFlyToTarget(ProjectilePresenter projectile)
        {
            this.RemoveProjectile(projectile);
        }

        protected virtual void OnProjectileHit(Collider2D collider2D, ProjectilePresenter projectile)
        {
            
        }

        protected void RemoveProjectile(ProjectilePresenter projectile)
        {
            if (this.firedProjectiles.Contains(projectile))
            {
                this.firedProjectiles.Remove(projectile);
            } 
        }
    }

    public class BaseProjectileSkillModel : IEntitySkillModel
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