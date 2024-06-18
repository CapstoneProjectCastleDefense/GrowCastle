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
            var startPos = this.Model.StartPos;
            for (var i = 0; i < this.Model.Number; i++)
            {
                var knightSummonObj = await this.objectPoolManager.Spawn(this.Model.PrefabName);
                knightSummonObj.transform.position                        =  startPos;
                startPos.y                                                -= this.Model.DistanceRange;
                knightSummonObj.GetComponent<MeshRenderer>().sortingOrder =  i + 1;
            }
        }
    }

    public class SummonSkillModel : IEntitySkillModel
    {
        public int     Number;
        public string  PrefabName;
        public Vector3 StartPos;
        public float   DistanceRange;
        public string  Id              { get; set; }
        public string  AddressableName { get; set; }
        public string  Description     { get; }
        public string  Name            { get; }
    }
}