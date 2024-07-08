namespace Runtime.Managers
{
    using System;
    using System.Collections.Generic;
    using Models.Tags;
    using Runtime.Interfaces.Entities;
    using Runtime.Systems.Effects;
    using UnityEngine;

    public class AffectManager
    {
        private readonly Dictionary<Type, IEffectSystem> affectSystems = new();

        public AffectManager(List<IEffectSystem> affectSystems)
        {
            affectSystems.ForEach(affect =>
            {
                this.affectSystems.Add(affect.ConditionFilterTag, affect);
                affect.AffectManager = this;
            });
        }

        public void AddAffectToTarget(ITargetable target, IElementTag tag, bool isForce = false)
        {
            if (!target.CurrentTag.ContainsKey(tag.GetType()))
            {
                target.CurrentTag.Add(tag.GetType(), tag);
                this.affectSystems[tag.GetType()].AffectedElements.Add(target);
            }

            if (!isForce) return;
            target.CurrentTag[tag.GetType()] = tag;
        }

        public void RemoveAffectOfTarget(ITargetable target, Type tagType)
        {
            if (!target.CurrentTag.ContainsKey(tagType)) return;
            target.CurrentTag.Remove(tagType);
            if (this.affectSystems[tagType].AffectedElements.Contains(target))
            {
                this.affectSystems[tagType].AffectedElements.Remove(target);
                Debug.Log($"Remove Affect {tagType.Name} from {target.GetType()}");
            }
        }
    }
}