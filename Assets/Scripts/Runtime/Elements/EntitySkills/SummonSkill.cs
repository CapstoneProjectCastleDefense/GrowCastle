namespace Runtime.Elements.EntitySkills
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Enums;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class SummonSkill : BaseEntitySkillPresenter<BasicSkillModel>
    {
        public override EntitySkillType SkillType { get; set; } = EntitySkillType.Summon;

        private readonly ObjectPoolManager    objectPoolManager;
        private readonly SkillSummonBlueprint skillSummonBlueprint;

        public SummonSkill(ObjectPoolManager objectPoolManager, SkillSummonBlueprint skillSummonBlueprint)
        {
            this.objectPoolManager    = objectPoolManager;
            this.skillSummonBlueprint = skillSummonBlueprint;
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
                var knightSummonObj = await this.objectPoolManager.Spawn(skillSummonRecord.PrefabName);
                knightSummonObj.transform.position                        =  startPos;
                startPos.y                                                -= skillSummonRecord.DistanceRange;
                knightSummonObj.GetComponent<MeshRenderer>().sortingOrder =  i + 1;
            }
        }
    }
}