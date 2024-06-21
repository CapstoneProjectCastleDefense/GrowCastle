namespace Runtime.Elements.EntitySkills
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Enums;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using UnityEngine;

    public class SummonSkill : BaseEntitySkillPresenter<BasicSkillModel>
    {
        public override EntitySkillType SkillType { get; set; } = EntitySkillType.Summon;

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
                this.summonerManager.CreateSingleSummoner(skillSummonRecord.SummonerId,startPos,i+1);
                startPos.y                                                -= skillSummonRecord.DistanceRange;
            }
        }
    }
}