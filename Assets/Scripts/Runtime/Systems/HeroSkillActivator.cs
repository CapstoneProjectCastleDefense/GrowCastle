namespace Runtime.Systems
{
    using System.Collections.Generic;
    using System.Linq;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class HeroSkillActivator : IGameSystem
    {
        private readonly Dictionary<string, IHeroSkill> idToHeroSkill;

        public HeroSkillActivator(IEnumerable<IHeroSkill> entitySkillPresenters) { this.idToHeroSkill = entitySkillPresenters.ToDictionary(entity => entity.SkillId, entity => entity); }

        public void Activate(string skillId, HeroPresenter hero)
        {
            if (this.idToHeroSkill.TryGetValue(skillId, out var heroSkill))
            {
                heroSkill.Activate(hero);
            }
            else
            {
                Debug.LogError($"[Skill system] Cannot find skill: {skillId}");
            }
        }

        public void Deactivate(string skillId, HeroPresenter hero)
        {
            if (this.idToHeroSkill.TryGetValue(skillId, out var heroSkill))
            {
                heroSkill.Deactivate(hero);
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