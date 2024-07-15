namespace Runtime.Elements.EntitySkills
{
    using Models.Blueprints;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.Services;
    using Runtime.StaticValues;
    using Runtime.Systems;
    using Zenject;

    public class SummonSkill : BaseEntitySkillPresenter<BasicSkillModel>
    {
        private readonly SkillSummonBlueprint skillSummonBlueprint;
        private readonly SummonerManager      summonerManager;

        public SummonSkill(SignalBus signalBus,
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

        public override string SkillId { get; set; } = EntitySkillName.SummonSkill;

        protected override void InternalActivate() { this.Summon(); }

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