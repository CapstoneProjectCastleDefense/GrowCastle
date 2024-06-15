namespace Runtime.Elements.Skills
{
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class SummonSkill : BaseEntitySkillPresenter<SummonKnightSkillModel>
    {
        private readonly ObjectPoolManager objectPoolManager;
        public override  string            SkillId => "SummonSkill";

        public SummonSkill(ObjectPoolManager objectPoolManager) { this.objectPoolManager = objectPoolManager; }

        public override async void Activate(BaseSkillModel skillModel)
        {
            base.Activate(skillModel);
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

    public class SummonKnightSkillModel : BaseSkillModel
    {
        public int     Number;
        public string  PrefabName;
        public Vector3 StartPos;
        public float   DistanceRange;
    }
}