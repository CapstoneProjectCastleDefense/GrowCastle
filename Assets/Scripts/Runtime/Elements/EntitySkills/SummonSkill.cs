namespace Runtime.Elements.EntitySkills
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;

    public abstract class SummonSkill : BaseEntitySkillPresenter<BasicSkillModel>
    {
        private readonly ObjectPoolManager    objectPoolManager;
        private readonly SkillSummonBlueprint skillSummonBlueprint;
        private readonly SummonerManager      summonerManager;

        public SummonSkill(ObjectPoolManager objectPoolManager, SkillSummonBlueprint skillSummonBlueprint,SummonerManager summonerManager)
        {
            this.objectPoolManager    = objectPoolManager;
            this.skillSummonBlueprint = skillSummonBlueprint;
            this.summonerManager      = summonerManager;
        }

        protected override void InternalActivate()
        {
            this.Summon().Forget();
        }

        private async UniTaskVoid Summon()
        {
            var skillSummonRecord = this.skillSummonBlueprint.GetDataById(this.Model.Id).SkillToLevelRecords[this.Model.Level];
            var startPos          = skillSummonRecord.StartPos;
            for (var i = 0; i < skillSummonRecord.NumberSpawn; i++)
            {
                this.summonerManager. CreateSingleSummoner(skillSummonRecord.SummonerId,startPos,i+1);
                startPos.y                                                -= skillSummonRecord.DistanceRange;
            }
        }
    }
}