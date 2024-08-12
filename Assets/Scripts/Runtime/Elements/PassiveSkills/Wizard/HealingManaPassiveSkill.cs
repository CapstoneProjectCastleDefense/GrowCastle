namespace Runtime.Elements.PassiveSkills.Wizard
{
    using System.Linq;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using UnityEngine;

    public class HealingManaPassiveSkill : IPassiveSkillPresenter
    {
        private readonly CastleManager castleManager;
        private readonly VFXService    vfxService;
        private          string        vfxName = "HealMana";
        public           HeroPresenter HeroPresenter { get; set; }

        public HealingManaPassiveSkill(CastleManager castleManager, VFXService vfxService)
        {
            this.castleManager = castleManager;
            this.vfxService    = vfxService;
        }
        public void Init()          { this.ActiveSkill(); }
        public void Tick()          { }
        public void ActiveSkill()   { this.HeroPresenter.OnActiveSkillCasted += this.HealingManaCastle; }
        public void DeActiveSkill() { this.HeroPresenter.OnActiveSkillCasted -= this.HealingManaCastle; }

        private void HealingManaCastle()
        {
            var castle                                                      = this.castleManager.entities.First();
            var maxMana                                                     = castle.Model.GetStat<float>(StatEnum.MaxMana);
            var mana                                                        = castle.Model.GetStat<float>(StatEnum.Mana) + maxMana * 0.15f;
            if (mana >= castle.Model.GetStat<float>(StatEnum.MaxMana)) mana = castle.Model.GetStat<float>(StatEnum.MaxMana);
            castle.Model.SetStat(StatEnum.Mana, mana);
            this.vfxService?.SpawnVFX(this.vfxName, new Vector3(3f, -2, 0), Quaternion.identity, scale: new Vector3(4, 4, 1));
        }
    }
}