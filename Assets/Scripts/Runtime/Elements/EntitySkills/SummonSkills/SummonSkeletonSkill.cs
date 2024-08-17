namespace Runtime.Elements.EntitySkills.SummonSkills
{
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Managers;
    using Runtime.StaticValues;

    public class SummonSkeletonSkill : SummonSkill
    {
        public SummonSkeletonSkill(ObjectPoolManager objectPoolManager, SkillSummonBlueprint skillSummonBlueprint, SummonerManager summonerManager) : base(objectPoolManager, skillSummonBlueprint, summonerManager)
        {
        }
        public override string SkillId { get; set; } = EntitySkillName.SkeletonArmy;
    }
}