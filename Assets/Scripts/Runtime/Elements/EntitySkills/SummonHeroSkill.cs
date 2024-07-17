namespace Runtime.Elements.EntitySkills
{
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using Zenject;

    public class SummonHeroSkill : BaseHeroSkill<BasicHeroSkillModel>
    {
        private readonly SkillSummonBlueprint skillSummonBlueprint;
        private readonly SummonerManager      summonerManager;

        public SummonHeroSkill(SignalBus signalBus,
                           FindTargetSystem findTargetSystem,
                           EffectManager effectManager,
                           VFXService vfxService,
                           SkillSummonBlueprint skillSummonBlueprint,
                           SummonerManager summonerManager)
            : base(signalBus, findTargetSystem, effectManager, vfxService)
        {
            this.skillSummonBlueprint = skillSummonBlueprint;
            this.summonerManager      = summonerManager;
        }

        public override string SkillId                        { get; set; } = EntitySkillName.SummonSkill;
        public override void   Activate(HeroPresenter caster) { this.Summon();  }
        public override void   Deactivate(HeroPresenter hero) {  }

        private void Summon()
        {
            var skillSummonRecord = this.skillSummonBlueprint.GetDataById(this.Model.Id).SkillToLevelRecords[this.Model.Level];
            var startPos          = skillSummonRecord.StartPos;
            for (var i = 0; i < skillSummonRecord.NumberSpawn; i++)
            {
                this.summonerManager.CreateSingleSummoner(skillSummonRecord.SummonerId, startPos, i + 1);
                startPos.y -= skillSummonRecord.DistanceRange;
            }
        }
    }
}