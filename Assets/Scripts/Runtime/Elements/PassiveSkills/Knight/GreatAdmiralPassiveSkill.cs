namespace Runtime.Elements.PassiveSkills.Knight
{
    using System.Linq;
    using Runtime.Elements.Entities.Hero;
    using Runtime.Elements.Entities.Summoner;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using Sirenix.Utilities;
    using UnityEngine;

    public class GreatAdmiralPassiveSkill : IPassiveSkillPresenter
    {
        private readonly SummonerManager summonerManager;
        public GreatAdmiralPassiveSkill(SummonerManager summonerManager) { this.summonerManager = summonerManager; }
        public HeroPresenter HeroPresenter { get; set; }
        public void          Init()        { this.ActiveSkill(); }
        public void          Tick()        { }
        public void ActiveSkill()
        {
            Debug.Log($"active skill {this.GetType().FullName}");
            this.summonerManager.OnCreateSummonerComplete += this.IncreaseAttackForKnightSummoner;
        }
        private void IncreaseAttackForKnightSummoner(SummonerPresenter summonerPresenter)
        {
            if (!summonerPresenter.Model.Id.Equals("SummonKnight")) return;
            var currentAtk = summonerPresenter.Model.GetStat<float>(StatEnum.Attack);
            summonerPresenter.Model.SetStat(StatEnum.Attack, currentAtk * 1.2f);
        }
        public void DeActiveSkill()
        {
            this.summonerManager.OnCreateSummonerComplete -= this.IncreaseAttackForKnightSummoner;
        }
    }
}