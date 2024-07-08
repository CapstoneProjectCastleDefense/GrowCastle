namespace Runtime.Systems.Effects
{
    using System;
    using Models.Tags;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Managers;
    using UnityEngine;

    public class BleedEffect : BaseEffect
    {
        public override Type EffectTagType => typeof(BleedTag);

        public EffectManager EffectManager { get; set; }

        public override void Initialize() { }

        public override void Tick()
        {
            this.Filter();
            foreach (var t in this.AffectedElements)
            {
                this.Bleed(t);
            }
        }

        private void Bleed(ITargetable target)
        {
            var tagData = (BleedTag)target.CurrentTag[this.EffectTagType];
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
                Debug.Log("Bleed: " + damage);
            }

            tagData.Timer += Time.deltaTime;
        }

        private void Filter()
        {
            //k convert sang foreach
            for (var index = 0; index < this.AffectedElements.Count; index++)
            {
                var target = this.AffectedElements[index];
                if (((BleedTag)target.CurrentTag[this.EffectTagType]).Duration <= 0)
                {
                    this.RemoveEffectOnTarget(target);
                }
            }
        }

        public override void Execute(ITargetable target, IElementTag tag) { this.AddEffectToTarget(target, tag); }

        public override void Dispose() { }
    }
}