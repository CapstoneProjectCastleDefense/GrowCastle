namespace Runtime.Managers
{
    using System;
    using System.Collections.Generic;
    using Models.Tags;
    using Runtime.Interfaces.Entities;
    using Runtime.Systems.Effects;
    using Zenject;

    public class EffectManager : ITickable, IDisposable
    {
        private readonly Dictionary<Type, IEffect> tagTypeToEffect = new();

        public EffectManager(List<IEffect> effects)
        {
            effects.ForEach(effect =>
            {
                this.tagTypeToEffect.Add(effect.EffectTagType, effect);
                effect.Initialize();
            });
        }

        public void Execute(ITargetable target, IEffectTag tag)
        {
            if (this.tagTypeToEffect.TryGetValue(tag.GetType(), out var effect))
            {
                effect.Execute(target, tag);
            }
        }
        
        public void Tick()
        {
            foreach (var effectSystem in this.tagTypeToEffect.Values)
            {
                effectSystem.Tick();
            }
        }
        public void Dispose()
        {
            foreach (var effect in this.tagTypeToEffect.Values)
            {
                effect.Dispose();
            }
        }
    }
}