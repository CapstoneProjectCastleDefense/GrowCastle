namespace Runtime.Elements.PassiveSkills.Knight
{
    using System.Linq;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Sirenix.Utilities;

    public class GreatAdmiralPassiveSkill : IPassiveSkillPresenter
    {
        private readonly SummonerManager summonerManager;
        public GreatAdmiralPassiveSkill(SummonerManager summonerManager) { this.summonerManager = summonerManager; }
        public HeroPresenter HeroPresenter { get; set; }
        public void          Init()        { this.ActiveSkill(); }
        public void          Tick()        { }
        public void ActiveSkill()
        {
            this.summonerManager.entities.Where(summoner => summoner.Model.Id.Equals("SummonKnight")).ForEach(e =>
            {
                var currentAtk = e.Model.GetStat<float>(StatEnum.Attack);
                e.Model.SetStat(StatEnum.Attack, currentAtk * 1.2f);
            });
        }
    }
}