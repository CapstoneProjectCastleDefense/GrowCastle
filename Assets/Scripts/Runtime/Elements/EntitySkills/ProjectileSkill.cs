namespace Runtime.Elements.EntitySkills
{
    using Cysharp.Threading.Tasks;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Enums;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class ProjectileSkill : BaseEntitySkillPresenter<ProjectileSkillModel>
    {
        private readonly ProjectileManager projectileManager;
        public ProjectileSkill(ProjectileManager projectileManager) { this.projectileManager = projectileManager; }

        public override EntitySkillType SkillType { get; set; } = EntitySkillType.Projectile;

        protected override void InternalActivate()
        {
            this.FireProjectile().Forget();
        }

        private async UniTaskVoid FireProjectile()
        {
            var projectile = this.projectileManager.CreateElement(new ProjectileModel
            {
                Id              = this.Model.Id,
                AddressableName = this.Model.AddressableName,
                Prefab          = this.Model.ProjectilePrefab,
                StartPoint      = this.Model.StartPoint,
                EndPoint        = this.Model.EndPoint
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
        public GameObject                           ProjectilePrefab;
        public Vector3                              StartPoint;
        public Vector3                              EndPoint;
    }
}