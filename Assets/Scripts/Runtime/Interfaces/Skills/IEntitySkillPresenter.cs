namespace Runtime.Interfaces.Skills
{
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.Signals;
    using Runtime.Systems;
    using Zenject;

    public interface IEntitySkillPresenter
    {
        string SkillId { get; set; }
        void   Activate(IEntitySkillModel baseSkillModel);
        void   Initialize();
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

        public virtual void Activate(IEntitySkillModel baseSkillModel)
        {
            if (baseSkillModel is TModel model)
            {
                this.Model = model;
            }

            this.InternalActivate();
        }

        public virtual void Initialize() { this.signalBus.Subscribe<TimeCooldownSignal>(this.Tick); }

        protected virtual void Tick(TimeCooldownSignal signal) { }

        public virtual void Dispose() { this.signalBus.Subscribe<TimeCooldownSignal>(this.Tick); }

        protected abstract void InternalActivate();
    }
}