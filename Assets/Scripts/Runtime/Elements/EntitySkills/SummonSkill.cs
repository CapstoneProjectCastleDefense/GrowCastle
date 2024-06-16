namespace Runtime.Elements.EntitySkills
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Enums;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class SummonSkill : BaseEntitySkillPresenter<SummonSkillModel>
    {
        public override EntitySkillType SkillType { get; set; } = EntitySkillType.Summon;
        
        private readonly ObjectPoolManager objectPoolManager;

        public SummonSkill(ObjectPoolManager objectPoolManager) { this.objectPoolManager = objectPoolManager; }

        protected override void InternalActivate()
        {
            this.Summon().Forget();
        }

        private async UniTaskVoid Summon()
        {
            //todo: summon base on data instead of knight only
            var startPos = this.SkillModel.StartPos;
            for (var i = 0; i < this.SkillModel.Number; i++)
            {
                var knightSummonObj = await this.objectPoolManager.Spawn(this.SkillModel.PrefabName);
                knightSummonObj.transform.position                        =  startPos;
                startPos.y                                                -= this.SkillModel.DistanceRange;
                knightSummonObj.GetComponent<MeshRenderer>().sortingOrder =  i + 1;
            }
        }
    }

    public class SummonSkillModel : BaseSkillModel
    {
        public int     Number;
        public string  PrefabName;
        public Vector3 StartPos;
        public float   DistanceRange;
    }
}