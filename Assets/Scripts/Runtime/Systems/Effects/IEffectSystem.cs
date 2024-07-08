namespace Runtime.Systems.Effects
{
    using System;
    using System.Collections.Generic;
    using Models.Tags;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using UnityEngine;

    public interface IEffectSystem : IGameSystem
    {
        Type              ConditionFilterTag { get; }
        List<ITargetable> AffectedElements   { get; set; }
        void              Filter();
        void              TriggerEffect(ITargetable target);
        AffectManager     AffectManager { set; }
    }

    public abstract class EffectSystem : IEffectSystem
    {
        public virtual void Initialize() { }

        public virtual void Tick() { }

        public virtual void Dispose() { }

        public abstract Type              ConditionFilterTag { get; }
        public virtual  List<ITargetable> AffectedElements   { get; set; }
        public virtual  void              Filter()           { }

        public virtual void TriggerEffect(ITargetable target) { }

        public void AddEffectToTarget(ITargetable target, IElementTag tag)
        {
            if (!target.CurrentTag.ContainsKey(this.ConditionFilterTag))
            {
                target.CurrentTag.Add(tag.GetType(), tag);
                this.AffectedElements.Add(target);
                return;
            }

            target.CurrentTag[tag.GetType()] = tag;
        }

        public void RemoveEffectOnTarget(ITargetable target)
        {
            if (!target.CurrentTag.ContainsKey(this.ConditionFilterTag)) return;
            target.CurrentTag.Remove(this.ConditionFilterTag);
            if (this.AffectedElements.Contains(target))
            {
                this.AffectedElements.Remove(target);
                Debug.Log($"Remove Affect {this.ConditionFilterTag.Name} from {target.GetType()}");
            }
        }

        public AffectManager AffectManager { get; set; }
    }
}