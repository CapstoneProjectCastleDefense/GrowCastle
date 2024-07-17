namespace Runtime.Systems
{
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class SkillActivator : IGameSystem
    {
        private readonly Dictionary<string, ISkill> idToHeroSkill;

        public SkillActivator(IEnumerable<ISkill> entitySkillPresenters) { this.idToHeroSkill = entitySkillPresenters.ToDictionary(entity => entity.SkillId, entity => entity); }

        public void Activate(string skillId, ICombatantPresenter combatant)
        {
            if (this.idToHeroSkill.TryGetValue(skillId, out var heroSkill))
            {
                heroSkill.Activate(combatant);
            }
            else
            {
                Debug.LogError($"[Skill system] Cannot find skill: {skillId}");
            }
        }

        public void Deactivate(string skillId, ICombatantPresenter combatant)
        {
            if (this.idToHeroSkill.TryGetValue(skillId, out var heroSkill))
            {
                heroSkill.Deactivate(combatant);
            }
            else
            {
                Debug.LogError($"[Skill system] Cannot find skill: {skillId}");
            }
        }

        public void Tick()
        {
            foreach (var entitySkill in this.idToHeroSkill)
            {
                entitySkill.Value.Tick();
            }
        }

        public void Initialize() { }

        public void Dispose() { ; }
    }
}