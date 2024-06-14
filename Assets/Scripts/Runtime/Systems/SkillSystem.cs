namespace Runtime.Systems
{
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class SkillSystem : IGameSystem
    {
        private readonly Dictionary<string, IEntitySkillPresenter> entitySkills;

        public SkillSystem(List<IEntitySkillPresenter> entitySkillPresenters)
        {
            this.entitySkills = entitySkillPresenters.ToDictionary(entity => entity.SkillId, entity => entity);
        }

        public void CastSkill(string skillId,BaseSkillModel skillModel)
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

        public void Dispose()
        {

        }

        public void Initialize()
        {

        }

        public void Tick()
        {

        }
    }
}