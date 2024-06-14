namespace Runtime
{
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Interfaces.Skills;
    using UnityEngine;

    public class SummonKnightSkill : BaseEntitySkillPresenter<SummonKnightSkillModel>
    {
        private readonly ObjectPoolManager objectPoolManager;
        public override  string            SkillId => "SummonKnight";

        public SummonKnightSkill(ObjectPoolManager objectPoolManager)
        {
            this.objectPoolManager = objectPoolManager;
        }

        public override async void Activate(BaseSkillModel skillModel)
        {
            base.Activate(skillModel);
            for (var i = 0; i < this.skillModel.numberKnight; i++)
            {
                var knightSummonObj = await this.objectPoolManager.Spawn(this.skillModel.prefabName);
                knightSummonObj.transform.position = Vector3.zero;
            }
        }
    }

    public class SummonKnightSkillModel : BaseSkillModel
    {
        public int    numberKnight;
        public string prefabName;
    }
}