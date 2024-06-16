namespace Runtime.Systems
{
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Enums;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class EntitySkillSystem : IGameSystem
    {
        private readonly Dictionary<EntitySkillType, IEntitySkillPresenter> entitySkills;

        public EntitySkillSystem(List<IEntitySkillPresenter> entitySkillPresenters) { this.entitySkills = entitySkillPresenters.ToDictionary(entity => entity.SkillType, entity => entity); }

        private EntitySkillType GetSkillType(string skillId) { return EntitySkillType.Summon; }

        public void CastSkill(string skillId, BaseSkillModel skillModel)
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