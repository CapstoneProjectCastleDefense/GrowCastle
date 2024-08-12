namespace Runtime.Elements.PassiveSkills.Wizard
{
    using System.Linq;
    using Models.Blueprints;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using UnityEngine;

    public class HealingHealthPassiveSkill : IPassiveSkillPresenter
    {
        private readonly CastleManager castleManager;
        private readonly VFXService    vfxService;
        private          string        vfxName = "HealOnceBurst";
        public           HeroPresenter HeroPresenter { get; set; }

        public HealingHealthPassiveSkill(CastleManager castleManager, VFXService vfxService)
        {
            this.castleManager = castleManager;
            this.vfxService    = vfxService;
        }
        public void Init()          { this.ActiveSkill(); }
        public void Tick()          { }
        public void ActiveSkill()   { this.HeroPresenter.OnActiveSkillCasted += this.HealingHealthCastle; }
        public void DeActiveSkill() { this.HeroPresenter.OnActiveSkillCasted -= this.HealingHealthCastle; }

        private void HealingHealthCastle()
        {
            var castle                                                            = this.castleManager.entities.First();
            var maxHealth                                                         = castle.Model.GetStat<float>(StatEnum.MaxHealth);
            var health                                                            = castle.Model.GetStat<float>(StatEnum.Health) + maxHealth * 0.15f;
            if (health >= castle.Model.GetStat<float>(StatEnum.MaxHealth)) health = castle.Model.GetStat<float>(StatEnum.MaxHealth);
            castle.Model.SetStat(StatEnum.Health, health);

            this.vfxService?.SpawnVFX(this.vfxName, new Vector3(3f, -2, 0), Quaternion.identity, scale: new Vector3(4, 4, 1));
        }
    }
}