namespace Runtime.Elements.EntitySkills.Knight
{
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.Systems;
    using UnityEngine;
    using Zenject;

    public class UndeadArmySkill : BaseSkill
    {
        private readonly SummonerManager summonerManager;

        public UndeadArmySkill(SignalBus signalBus,
            FindTargetSystem findTargetSystem,
            EffectManager effectManager,
            VFXService vfxService,
            SummonerManager summonerManager)
            : base(signalBus, findTargetSystem, effectManager, vfxService)
        {
            this.summonerManager = summonerManager;
        }

        public override string SkillId { get; set; } = "Knight_SK1";

        public override void Activate(ICombatantPresenter combatant)
        {
            var hero = (HeroPresenter)combatant;
            hero.OnClickAction = this.Execute;
        }

        private void Execute()
        {
            var startPos = UndeadArmySkillData.StartPos;
            for (var i = 0; i < UndeadArmySkillData.NumberSpawn; i++)
            {
                this.summonerManager.CreateSingleSummoner(UndeadArmySkillData.SummonerId, startPos, i + 1);
                startPos.y -= UndeadArmySkillData.DistanceRange;
            }
        }

        public override void Deactivate(ICombatantPresenter combatant)
        {
            var hero = (HeroPresenter)combatant;
            hero.OnClickAction = null;
        }
    }

    public static class UndeadArmySkillData
    {
        public const  string  SummonerId    = "SummonKnight";
        public const  int     NumberSpawn   = 2;
        public static Vector3 StartPos      = new Vector3(2, -2, 0);
        public const  float   DistanceRange = .5f;
    }
}