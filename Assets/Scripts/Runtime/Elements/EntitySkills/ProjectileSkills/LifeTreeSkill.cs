namespace Runtime.Elements.EntitySkills.ProjectileSkills
{
    using GameFoundation.Scripts.AssetLibrary;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Projectile;
    using Runtime.Elements.Entities.Tower;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.StaticValues;

    public class LifeTreeSkill : BaseProjectileSkill<BaseProjectileSkillModel>
    {
        private readonly EnemyManager       enemyManager;
        private readonly HeroUpgradeService heroUpgradeService;
        private readonly VFXService         vfxService;
        private          string             increaseEffectName = "LifeTree";
        public LifeTreeSkill(
            ProjectileManager projectileManager,
            IGameAssets gameAssets,
            ProjectileBlueprint projectileBlueprint,
            EffectManager effectManager,
            EnemyManager enemyManager,
            HeroUpgradeService heroUpgradeService,
            VFXService vfxService)
            : base(projectileManager, gameAssets, projectileBlueprint, effectManager)
        {
            this.enemyManager       = enemyManager;
            this.heroUpgradeService = heroUpgradeService;
            this.vfxService         = vfxService;
        }
        public override string SkillId { get; set; } = EntitySkillName.LifeTreeSkill;

        protected override void InternalActivate()
        {
            this.vfxService.SpawnVFX("ItemSparkleBurstRainbow", ((TowerView)this.Model.Caster.GetView()).spawnProjectilePos.position);
            if (this.enemyManager.IncreaseExpDropPercent.ContainsKey(this.increaseEffectName))
            {
                this.enemyManager.IncreaseExpDropPercent[this.increaseEffectName] = this.heroUpgradeService.GetCurrentAttack("LifeTree");
                return;
            }

            this.enemyManager.IncreaseExpDropPercent.Add(this.increaseEffectName, this.heroUpgradeService.GetCurrentAttack("LifeTree"));
        }
    }
}