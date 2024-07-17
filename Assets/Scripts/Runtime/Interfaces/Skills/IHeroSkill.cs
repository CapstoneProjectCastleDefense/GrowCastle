namespace Runtime.Interfaces.Skills
{
    using System;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.Signals;
    using Runtime.Systems;
    using Zenject;

    public interface IHeroSkill
    {
        string SkillId { get; set; }
        void   Activate(HeroPresenter hero);
        void   Deactivate(HeroPresenter hero);
        void   Tick();
    }

    public abstract class BaseHeroSkill<TModel> : IHeroSkill where TModel : IHeroSkillModel
    {
        #region Inject

        protected readonly SignalBus        signalBus;
        protected readonly FindTargetSystem findTargetSystem;
        protected readonly EffectManager    effectManager;
        protected readonly VFXService       vfxService;

        public BaseHeroSkill(SignalBus signalBus, FindTargetSystem findTargetSystem, EffectManager effectManager, VFXService vfxService)
        {
            this.signalBus        = signalBus;
            this.findTargetSystem = findTargetSystem;
            this.effectManager    = effectManager;
            this.vfxService       = vfxService;
        }

        #endregion

        public abstract string SkillId { get; set; }

        protected TModel Model;

        public abstract void Activate(HeroPresenter caster);
        public abstract void Deactivate(HeroPresenter hero);

        public virtual void Initialize() { this.signalBus.Subscribe<TimeCooldownSignal>(this.Tick); }

        public virtual void Tick() { }

        public virtual Type[] GetManagerTypes() { return new[] { typeof(EnemyManager), typeof(CastleManager) }; }

        public virtual string[] GetTags() { return new[] { "Fly", "Ground", "Boss", "Building" }; }

        public virtual void Dispose() { this.signalBus.Subscribe<TimeCooldownSignal>(this.Tick); }
    }
}