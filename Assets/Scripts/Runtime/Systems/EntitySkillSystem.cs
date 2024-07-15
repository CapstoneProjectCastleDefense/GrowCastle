namespace Runtime.Systems
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class EntitySkillSystem : IGameSystem
    {
        private readonly SkillBlueprint                            skillBlueprint;
        private readonly Dictionary<string, IEntitySkillPresenter> entitySkills;

        public EntitySkillSystem(List<IEntitySkillPresenter> entitySkillPresenters, SkillBlueprint skillBlueprint)
        {
            this.skillBlueprint = skillBlueprint;
            this.entitySkills   = entitySkillPresenters.ToDictionary(entity => entity.SkillId, entity => entity);
        }

        public void CastSkill(string skillId, ICombatantPresenter caster)
        {
            if (this.entitySkills.TryGetValue(skillId, out var entitySkill))
            {
                entitySkill.Cast(caster);
            }
            else
            {
                Debug.LogError($"[Skill system] Cannot find skill: {skillId}");
            }
        }

        public void Dispose()
        {
            foreach (var entitySkill in this.entitySkills)
            {
                entitySkill.Value.Dispose();
            }
        }

        public void Initialize()
        {
            foreach (var entitySkill in this.entitySkills)
            {
                entitySkill.Value.Initialize();
            }
        }

        public void Tick()
        {
            foreach (var entitySkill in this.entitySkills)
            {
                entitySkill.Value.Tick();
            }
        }
    }
}