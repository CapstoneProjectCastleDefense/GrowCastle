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
        private readonly ProjectileManager   projectileManager;
        private readonly AbilitySystem       abilitySystem;
        private readonly AffectManager       affectManager;

        public BaseProjectileSkill(ProjectileManager projectileManager,
                                   IGameAssets gameAssets,
                                   ProjectileBlueprint projectileBlueprint,
                                   AbilitySystem abilitySystem, AffectManager affectManager)
        {
            this.projectileManager   = projectileManager;
            this.abilitySystem       = abilitySystem;
            this.affectManager       = affectManager;
        }

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
            projectile.FlyToTarget().onComplete += this.OnFlyToTarget;
        }

        private void OnFlyToTarget()
        {
            this.Model.Target.OnGetHit(this.Model.Damage);
            this.abilitySystem.Execute(AbilityName.DealDamage, this.Model.Target, new Dictionary<StatEnum, (Type, object)>
            {
                { StatEnum.Attack, (typeof(float), this.Model.Damage) }
            });
            this.affectManager.AddAffectToTarget(this.Model.Target,new BleedTag(){Duration = 0.2f,TimeDelay = 0.1f,Timer = 0});
        }

        public virtual void OnProjectileHit(Collider2D collider2D)
        {
            this.Model.Target.OnGetHit(this.Model.Damage);
            this.abilitySystem.Execute(AbilityName.DealDamage, this.Model.Target, new Dictionary<StatEnum, (Type, object)>()
            {
                { StatEnum.Attack, (typeof(float), this.Model.Damage) }
            });
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