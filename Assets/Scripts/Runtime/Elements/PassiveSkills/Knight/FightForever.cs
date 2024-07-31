namespace Runtime.Elements.PassiveSkills.Knight
{
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;

    public class FightForever : IPassiveSkillPresenter
    {
        private readonly SummonerManager summonerManager;
        public FightForever(SummonerManager summonerManager) { this.summonerManager = summonerManager; }
        public HeroPresenter HeroPresenter { get; set; }
        public void          Init()        { this.ActiveSkill(); }
        public void          Tick()        { }
        public void ActiveSkill()
        {
            this.summonerManager.entities.ForEach(summoner =>
            {
                var currentAtk = summoner.Model.GetStat<float>(StatEnum.Attack);
                summoner.Model.SetStat(StatEnum.Attack, currentAtk * 1.2f);
            });
        }
    }
}