namespace Runtime.Elements.PassiveSkills.GreenLeaf
{
    using Models.Tags;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;

    public class ElvenMarkPassiveSkill : IPassiveSkillPresenter
    {
        private readonly EffectManager effectManager;
        private readonly VFXService    vfxService;
        private          string        vfxName = "EnergyExplosionGreen";

        public ElvenMarkPassiveSkill(EffectManager effectManager, VFXService vfxService)
        {
            this.effectManager = effectManager;
            this.vfxService    = vfxService;
        }

        public HeroPresenter HeroPresenter { get; set; }

        public void Init()
        {
            this.ActiveSkill();
        }

        public void Tick()
        {
        }

        public void ActiveSkill()
        {
            this.HeroPresenter.OnAttackComplete += this.DealMoreDamage;
        }

        private void DealMoreDamage(ITargetable target)
        {
            this.vfxService?.SpawnVFX(this.vfxName, target.GetGameObject().transform.position);
            this.effectManager.AddEffectToTarget(target, new InstantDamageTag() { Damage = this.HeroPresenter.Model.GetStat<float>(StatEnum.Attack) * 0.3f });
        }

        public void DeActiveSkill()
        {
            this.HeroPresenter.OnAttackComplete -= this.DealMoreDamage;
        }
    }
}