namespace Runtime.Managers
{
    using System;
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Summoner;
    using Runtime.Elements.EntitySkills;
    using Runtime.Elements.EntitySkills.SummonSkills;
    using Runtime.Enums;
    using Runtime.Managers.Base;
    using UnityEngine;

    public class SummonerManager : BaseElementManager<SummonerModel, SummonerPresenter, SummonerView>
    {
        private readonly SummonerBlueprint summonerBlueprint;
        public SummonerManager(BaseElementPresenter<SummonerModel, SummonerView, SummonerPresenter>.Factory factory, SummonerBlueprint summonerBlueprint)
            : base(factory)
        {
            this.summonerBlueprint = summonerBlueprint;
        }
        public Action<SummonerPresenter> OnCreateSummonerComplete;

        public void CreateSingleSummoner(string summonerId, Vector3 startPos, int sortingIndex, float timeExist, float attack ,float attackSpeed)
        {
            var summonerRecord = this.summonerBlueprint.GetDataById(summonerId);
            var presenter = this.CreateElement(new()
            {
                Id              = summonerId,
                AddressableName = summonerRecord.PrefabName,
                StartPos        = startPos,
                SortingIndex    = sortingIndex,
                BaseStats = new() //TODO : Replace with data from blueprint
                {
                    { StatEnum.Attack, (typeof(float), attack) },
                    { StatEnum.AttackSpeed, (typeof(float), attackSpeed) },
                    { StatEnum.Health, (typeof(float), 2000f) },
                    { StatEnum.MoveSpeed, (typeof(float), 2f) },
                    { StatEnum.AttackRange, (typeof(float), 1f) },
                    { StatEnum.ExistTime, (typeof(float), timeExist) },
                    { StatEnum.MaxExistTime, (typeof(float), timeExist) }
                }
            });
            this.OnCreateSummonerComplete?.Invoke(presenter);
            presenter.UpdateView().Forget();
        }
        public override void Initialize() { }
    }
}