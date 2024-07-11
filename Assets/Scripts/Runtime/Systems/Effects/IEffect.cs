namespace Runtime.Systems.Effects
{
    using System;
    using System.Collections.Generic;
    using Models.Tags;
    using Runtime.Interfaces.Entities;
    using Runtime.StaticValues;
    using UnityEngine;

    public interface IEffect
    {
        Type              EffectTagType    { get; }
        List<ITargetable> AffectedElements { get; }
        void              Initialize();
        void              Tick();
        void              Dispose();
        void              Execute(ITargetable target, IEffectTag tag);
    }

    public abstract class BaseEffect : IEffect
    {
        public virtual void Initialize() { }
        public virtual void Tick()
        {
            this.Filter();
            for (int i = 0; i < this.AffectedElements.Count; i++)
            {
                this.ActiveEffect(this.AffectedElements[i]);
            }
        }

        protected abstract void Filter();
        protected abstract void ActiveEffect(ITargetable target);

        public virtual void Dispose() { }

        public abstract Type              EffectTagType    { get; }
        public          List<ITargetable> AffectedElements { get; set; } = new();
        public abstract void              Execute(ITargetable target, IEffectTag tag);

        public void AddEffectToTarget(ITargetable target, IEffectTag tag)
        {
            if (!target.CurrentTag.ContainsKey(this.EffectTagType))
            {
                target.CurrentTag.Add(tag.GetType(), tag);
                this.AffectedElements.Add(target);
                return;
            }

            target.CurrentTag[tag.GetType()] = tag;
        }

        public void RemoveEffectOnTarget(ITargetable target)
        {
            if (!target.CurrentTag.ContainsKey(this.EffectTagType)) return;
            target.CurrentTag.Remove(this.EffectTagType);
            if (this.AffectedElements.Contains(target))
            {
                this.AffectedElements.Remove(target);
                Debug.Log($"Remove Affect {this.EffectTagType.Name} from {target.GetType()}");
            }
        }
    }
}