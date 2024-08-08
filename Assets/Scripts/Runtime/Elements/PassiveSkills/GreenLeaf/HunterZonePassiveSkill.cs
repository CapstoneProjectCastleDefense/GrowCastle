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

    public class HunterZonePassiveSkill : IPassiveSkillPresenter
    {
        private readonly EffectManager effectManager;
        private readonly VFXService    vfxService;
        private          string        vfxName = "EnergyExplosionPink";

        public HunterZonePassiveSkill(EffectManager effectManager, VFXService vfxService)
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
            this.HeroPresenter.OnAttackComplete += this.TriggerEffect;
        }

        private void TriggerEffect(ITargetable target)
        {
            this.vfxService?.SpawnVFX(this.vfxName, target.GetGameObject().transform.position);
            this.effectManager.AddEffectToTarget(target, new InstantDamageTag() { Damage = this.HeroPresenter.Model.GetStat<float>(StatEnum.Attack) * 0.1f });
            this.effectManager.AddEffectToTarget(target, new SlowTag() { Duration        = 0.5f, Timer = 0 });
        }

        public void DeActiveSkill()
        {
            this.HeroPresenter.OnAttackComplete -= this.TriggerEffect;
        }
    }
}