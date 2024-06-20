namespace Runtime.Elements.EntitySkills
{
    using System;
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Enums;
    using Runtime.Executors;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class ProjectileSkill : BaseEntitySkillPresenter<ProjectileSkillModel>
    {
        private readonly ProjectileManager         projectileManager;
        private readonly IGameAssets               gameAssets;
        private readonly ProjectileBlueprint       projectileBlueprint;
        private readonly EntitySkillEffectExecutor entitySkillEffectExecutor;
        private readonly SkillBlueprint            skillBlueprint;

        public ProjectileSkill(ProjectileManager projectileManager,
            IGameAssets gameAssets,
            ProjectileBlueprint projectileBlueprint,
            EntitySkillEffectExecutor entitySkillEffectExecutor,
            SkillBlueprint skillBlueprint)
        {
            this.projectileManager         = projectileManager;
            this.gameAssets                = gameAssets;
            this.projectileBlueprint       = projectileBlueprint;
            this.entitySkillEffectExecutor = entitySkillEffectExecutor;
            this.skillBlueprint            = skillBlueprint;
        }

        public override EntitySkillType SkillType { get; set; } = EntitySkillType.Projectile;

        protected override void InternalActivate() { this.FireProjectile().Forget(); }

        private async UniTaskVoid FireProjectile()
        {
            var projectileSkillRecord = this.projectileBlueprint.GetDataById(this.Model.Id);
            var projectile = this.projectileManager.CreateElement(new()
            {
                Id              = this.Model.Id,
                AddressableName = this.Model.AddressableName,
                Prefab          = this.gameAssets.LoadAssetAsync<GameObject>(projectileSkillRecord.PrefabName).WaitForCompletion(),
                StartPoint      = this.Model.StartPoint,
                EndPoint        = this.Model.EndPoint,
                Damage          = this.Model.damage,
            });

            await projectile.UpdateView();
            projectile.FlyToTarget(this.Model.Target);
        }
    }

    public class ProjectileSkillModel : IEntitySkillModel
    {
        public string      Id              { get; set; }
        public string      AddressableName { get; set; }
        public string      Description     { get; }
        public string      Name            { get; }
        public Vector3     StartPoint;
        public Vector3     EndPoint;
        public ITargetable Target;
        public float       damage;
    }
}