namespace Runtime.Elements.EntitySkillEffect
{
    using System;
    using System.Collections.Generic;
    using Runtime.Enums;
    using Runtime.Interfaces.Entities;

    public class LongLastingSkillEffect : IEntitySkillEffect
    {
        public EntitySkillEffectType EntitySkillEffectType { get; set; } = EntitySkillEffectType.LongLasting;

        public void Execute(ITargetable target, Dictionary<StatEnum, (Type, object)> stats)
        {
            
        }

        public void Tick(float deltaTime) {  }
    }
}