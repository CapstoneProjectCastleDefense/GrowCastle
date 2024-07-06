namespace Runtime.Systems
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Blueprints;
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


        public void CastSkill(string skillId, IEntitySkillModel skillModel)
        {
            if (this.entitySkills.TryGetValue(skillId, out var entitySkill))
            {
                entitySkill.Activate(skillModel);
            }
            else
            {
                Debug.LogError($"[Skill system] Cannot find skill: {skillId}");
            }
        }

        public void Dispose() { }

        public void Initialize() { }

        public void Tick() { }
    }
}