namespace Runtime.Elements.EntitySkills.ProjectileSkills
{
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Elements.Entities.Tower;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.StaticValues;

    public class GoldenStatueSkill : BaseProjectileSkill<BaseProjectileSkillModel>
    {
        private readonly EnemyManager       enemyManager;
        private readonly HeroUpgradeService heroUpgradeService;
        private readonly VFXService         vfxService;
        private          string             increaseEffectName = "GoldenStatue";
        public GoldenStatueSkill(ProjectileManager projectileManager, IGameAssets gameAssets, ProjectileBlueprint projectileBlueprint, EffectManager effectManager, EnemyManager enemyManager, HeroUpgradeService heroUpgradeService, VFXService vfxService)
            : base(projectileManager, gameAssets, projectileBlueprint, effectManager)
        {
            this.enemyManager       = enemyManager;
            this.heroUpgradeService = heroUpgradeService;
            this.vfxService         = vfxService;
        }
        public override string SkillId { get; set; } = EntitySkillName.GoldenStatueSkill;

        protected override void InternalActivate()
        {
            this.vfxService.SpawnVFX("ItemSparkleBurstYellow", ((TowerView)this.Model.Caster.GetView()).spawnProjectilePos.position);
            if (this.enemyManager.InCreaseGoldDropPercent.ContainsKey(this.increaseEffectName))
            {
                this.enemyManager.InCreaseGoldDropPercent[this.increaseEffectName] = this.heroUpgradeService.GetCurrentAttack("GoldenStatue");
                return;
            }

            this.enemyManager.InCreaseGoldDropPercent.Add(this.increaseEffectName, this.heroUpgradeService.GetCurrentAttack("GoldenStatue"));
        }
    }
}