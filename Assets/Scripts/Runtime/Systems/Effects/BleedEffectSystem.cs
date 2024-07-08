namespace Runtime.Systems.Effects
{
    using System;
    using System.Collections.Generic;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using UnityEngine;

    public class BleedEffectSystem : IEffectSystem
    {
        public Type              ConditionFilterTag => typeof(BleedTag);
        public List<ITargetable> AffectedElements   { get; set; } = new();

        public AffectManager AffectManager { get; set; }

        public void Initialize() { }

        public void Tick()
        {
            this.Filter();
            foreach (var t in this.AffectedElements)
            {
                this.TriggerEffect(t);
            }
        }
        public void TriggerEffect(ITargetable target)
        {
            var tagData = (BleedTag)target.CurrentTag[this.ConditionFilterTag];
            if (tagData.Timer >= tagData.TimeDelay)
            {
                var targetStats = target.GetStats();
                var health      = targetStats.GetStat<float>(StatEnum.Health);
                var damage      = health * 0.02f;
                if (health <= 0)
                {
                    return;
                }

                health -= damage;

                targetStats.SetStat(StatEnum.Health, health);
                target.OnGetHit(0f);
                tagData.Duration -= tagData.TimeDelay;
                tagData.Timer    =  0;
                Debug.Log("Bleed: "+damage);
            }

            tagData.Timer += Time.deltaTime;
        }

        public void Filter()
        {
            for (var index = 0; index < this.AffectedElements.Count; index++)
            {
                var target = this.AffectedElements[index];
                if (((BleedTag)target.CurrentTag[this.ConditionFilterTag]).Duration <= 0)
                {
                    this.AffectManager.RemoveAffectOfTarget(target, typeof(BleedTag));
                }
            }
        }
        public void Dispose() { }
    }
}