namespace Runtime.Elements.PassiveSkills.ARise
{
    using System.Linq;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Sirenix.Utilities;

    public class ShadowLord : IPassiveSkillPresenter
    {
        private readonly SummonerManager summonerManager;
        private readonly CastleManager   castleManager;
        public           HeroPresenter   HeroPresenter { get; set; }

        public ShadowLord(SummonerManager summonerManager, CastleManager castleManager)
        {
            this.summonerManager = summonerManager;
            this.castleManager   = castleManager;
        }

        public void Init()
        {
            this.HeroPresenter.OnActiveSkillCasted += this.ActiveSkill;
        }

        public void Tick()
        {
        }

        public void ActiveSkill()
        {
            this.summonerManager.entities.Where(e => e.Model.Id.Equals("SummonSkeleton")).ForEach(e => e.Model.OnSummonerDeath = this.OnSkeletonSummonerDeath);
        }
        public void DeActiveSkill()
        {
            this.HeroPresenter.OnActiveSkillCasted -= this.ActiveSkill;
        }

        private void OnSkeletonSummonerDeath()
        {
            this.castleManager.ReceiveMana(10);
        }
    }
}