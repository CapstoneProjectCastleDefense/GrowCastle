namespace Runtime.Elements.PassiveSkills.Knight
{
    using Runtime.Elements.Entities.Hero;
    using Runtime.Elements.Entities.Summoner;
    using Runtime.Enums;
    using Runtime.Extensions;
    using Runtime.Interfaces.Skills;
    using Runtime.Managers;
    using UnityEngine;

    public class FightForeverPassiveSkill : IPassiveSkillPresenter
    {
        private readonly SummonerManager summonerManager;
        public FightForeverPassiveSkill(SummonerManager summonerManager) { this.summonerManager = summonerManager; }
        public HeroPresenter HeroPresenter { get; set; }
        public void          Init()        { this.ActiveSkill(); }
        public void          Tick()        { }
        public void ActiveSkill()
        {
            Debug.Log($"active skill {this.GetType().FullName}");
            this.summonerManager.OnCreateSummonerComplete += this.IncreaseAttackForSummoner;

        }

        private void IncreaseAttackForSummoner(SummonerPresenter summonerPresenter)
        {
            var currentAtk = summonerPresenter.Model.GetStat<float>(StatEnum.Attack);
            summonerPresenter.Model.SetStat(StatEnum.Attack, currentAtk * 1.2f);
        }
        public void DeActiveSkill()
        {
            this.summonerManager.OnCreateSummonerComplete -= this.IncreaseAttackForSummoner;
        }
    }
}