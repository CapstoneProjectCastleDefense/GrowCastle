namespace Runtime.Elements.EntitySkills.SummonSkills
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Entities;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Runtime.StaticValues;

    public class SummonSkill : BaseEntitySkillPresenter<BasicSkillModel>
    {
        private readonly ObjectPoolManager    objectPoolManager;
        private readonly SkillSummonBlueprint skillSummonBlueprint;
        private readonly SummonerManager      summonerManager;

        public override string SkillId { get; set; } = EntitySkillName.SummonSkill;
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
            var casterStat        = ((ITargetable)this.Model.Caster).GetStats();
            for (var i = 0; i < skillSummonRecord.NumberSpawn; i++)
            {
                this.summonerManager. CreateSingleSummoner(skillSummonRecord.SummonerId,startPos,i+1,skillSummonRecord.TimeExist,casterStat.GetStat<float>(StatEnum.Attack),casterStat.GetStat<float>(StatEnum.AttackSpeed));
                startPos.y                                                -= skillSummonRecord.DistanceRange;
            }
        }
    }
}