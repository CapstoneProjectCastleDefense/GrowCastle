namespace Runtime.Systems
{
    using System.Collections.Generic;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class SkillSystem
    {
        private readonly Dictionary<string, IEntitySkillPresenter> EntitySkills;

        public SkillSystem(List<IEntitySkillPresenter> entitySkillPresenters)
        {
            // this.EntitySkills = entitySkillPresenters.ToDictionary(s => s.);
        }

        public void CastSkill(string skillId)
        {
            if (this.EntitySkills.TryGetValue(skillId, out var entitySkill))
            {
                entitySkill.Activate();
            }
            else
            {
                Debug.LogError($"[Skill system] Cannot find skill: {skillId}");
            }
        }
    }
}