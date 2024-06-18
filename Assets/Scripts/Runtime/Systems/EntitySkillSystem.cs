namespace Runtime.Systems
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Blueprints;
    using Runtime.Enums;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class EntitySkillSystem : IGameSystem
    {
        private readonly SkillBlueprint                                     skillBlueprint;
        private readonly Dictionary<EntitySkillType, IEntitySkillPresenter> entitySkills;

        public EntitySkillSystem(List<IEntitySkillPresenter> entitySkillPresenters, SkillBlueprint skillBlueprint)
        {
            this.skillBlueprint = skillBlueprint;
            this.entitySkills   = entitySkillPresenters.ToDictionary(entity => entity.SkillType, entity => entity);
        }

        private EntitySkillType GetSkillType(string skillId)
        {
            return this.skillBlueprint.GetDataById(skillId).Type;
        }

        public void CastSkill(string skillId, IEntitySkillModel skillModel)
        {
            if (this.entitySkills.TryGetValue(this.GetSkillType(skillId), out var entitySkill))
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