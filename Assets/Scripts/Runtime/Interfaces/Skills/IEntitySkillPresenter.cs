namespace Runtime.Interfaces.Skills
{
    using System;
    using Runtime.Elements.Base;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.Signals;
    using Runtime.Systems;
    using Zenject;

    public interface IEntitySkillPresenter
    {
        string SkillId { get; set; }
        void   Cast(ICombatantPresenter caster);
        void   Initialize();
        void   Tick();
        void   Dispose();
    }

    public abstract class BaseEntitySkillPresenter<TModel> : IEntitySkillPresenter where TModel : IEntitySkillModel
    {
        #region Inject

        protected readonly SignalBus        signalBus;
        protected readonly FindTargetSystem findTargetSystem;
        protected readonly EffectManager    effectManager;
        protected readonly VFXService       vfxService;

        public BaseEntitySkillPresenter(SignalBus signalBus, FindTargetSystem findTargetSystem, EffectManager effectManager, VFXService vfxService)
        {
            this.signalBus        = signalBus;
            this.findTargetSystem = findTargetSystem;
            this.effectManager    = effectManager;
            this.vfxService       = vfxService;
        }

        #endregion

        public abstract string SkillId { get; set; }

        protected TModel Model;

        public abstract void Cast(ICombatantPresenter caster);

        public virtual void Initialize() { this.signalBus.Subscribe<TimeCooldownSignal>(this.Tick); }

        public virtual void Tick() { }

        public virtual Type[] GetManagerTypes() { return new[] { typeof(EnemyManager), typeof(CastleManager) }; }
        
        public virtual string[] GetTags() { return new[] { "Fly", "Ground", "Boss", "Building" }; }

        public virtual void Dispose() { this.signalBus.Subscribe<TimeCooldownSignal>(this.Tick); }
    }
}