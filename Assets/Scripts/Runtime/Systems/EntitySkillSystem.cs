namespace Runtime.Systems
{
    using System.Collections.Generic;
    using System.Linq;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Interfaces.Skills;
    using Runtime.StaticValues;
    using UnityEngine;
    using Zenject;

    public class EntitySkillSystem : IGameSystem
    {
        private readonly SkillBlueprint                            skillBlueprint;
        private readonly DiContainer                               diContainer;
        private readonly Dictionary<string, IEntitySkillPresenter> entitySkills;

        public EntitySkillSystem(List<IEntitySkillPresenter> entitySkillPresenters, SkillBlueprint skillBlueprint, DiContainer diContainer)
        {
            this.skillBlueprint = skillBlueprint;
            this.diContainer    = diContainer;
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

        public void ActivePassiveSkill(string skillId, HeroPresenter heroPresenter)
        {
            var passiveSkill = (IPassiveSkillPresenter)this.diContainer.Instantiate(PassiveSkillName.PassiveSkillPresenters[skillId]);
            passiveSkill.HeroPresenter = heroPresenter;
            heroPresenter.PassiveSkillPresenters.Add(passiveSkill);
            passiveSkill.Init();
        }


        public void Dispose() { }

        public void Initialize() { }

        public void Tick() { }
    }
}