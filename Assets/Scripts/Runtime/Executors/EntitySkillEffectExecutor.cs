namespace Runtime.Executors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Elements.EntitySkillEffect;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;
    using UnityEngine;
    using Zenject;

    public class EntitySkillEffectExecutor : ITickable
    {
        private readonly Dictionary<EntitySkillEffectType, IEntitySkillEffect> typeToEntitySkillEffects;
        
        public EntitySkillEffectExecutor(IEntitySkillEffect[] entitySkillEffects)
        {
            this.typeToEntitySkillEffects = entitySkillEffects.ToDictionary(x => x.EntitySkillEffectType, x => x);
        }

        public void Execute(EntitySkillEffectType skillEffectType, ITargetable target, Dictionary<StatEnum, (Type, object)> stats)
        {
            if (this.typeToEntitySkillEffects.TryGetValue(skillEffectType, out var entitySkillEffect))
            {
                entitySkillEffect.Execute(target, stats);
            }
        }
        
        public void Tick()
        {
            foreach (var entitySkillEffect in this.typeToEntitySkillEffects.Values)
            {
                entitySkillEffect.Tick(Time.deltaTime);
            }
        }
    }
}