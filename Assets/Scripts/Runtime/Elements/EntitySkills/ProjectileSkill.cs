namespace Runtime.Elements.EntitySkills
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Enums;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class ProjectileSkill : BaseEntitySkillPresenter<ProjectileSkillModel>
    {
        private readonly ProjectileManager   projectileManager;
        private readonly IGameAssets         gameAssets;
        private readonly ProjectileBlueprint projectileBlueprint;

        public ProjectileSkill(ProjectileManager projectileManager, IGameAssets gameAssets,ProjectileBlueprint projectileBlueprint)
        {
            this.projectileManager   = projectileManager;
            this.gameAssets          = gameAssets;
            this.projectileBlueprint = projectileBlueprint;
        }

        public override EntitySkillType SkillType { get; set; } = EntitySkillType.Projectile;

        protected override void InternalActivate()
        {
            this.FireProjectile().Forget();
        }

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
            });

            await projectile.UpdateView();
            projectile.FlyToTarget();
        }
    }

    public class ProjectileSkillModel : IEntitySkillModel
    {
        public string                               Id              { get; set; }
        public string                               AddressableName { get; set; }
        public string                               Description     { get; }
        public string                               Name            { get; }
        public Vector3                              StartPoint;
        public Vector3                              EndPoint;
    }
}