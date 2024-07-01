namespace Runtime.Managers
{
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Summoner;
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

        public void CreateSingleSummoner(string summonerId, Vector3 startPos, int sortingIndex)
        {
            var summonerRecord = this.summonerBlueprint.GetDataById(summonerId);
            var presenter = this.CreateElement(new()
            {
                Id              = summonerId,
                AddressableName = summonerRecord.PrefabName,
                StartPos        = startPos,
                SortingIndex    = sortingIndex,
                Stats = new() //TODO : Replace with data from blueprint
                {
                    { StatEnum.Attack, (typeof(float), 10f) },
                    { StatEnum.AttackSpeed, (typeof(float), 1f) },
                    { StatEnum.Health, (typeof(float), 10f) },
                    { StatEnum.MoveSpeed, (typeof(float), 10f) },
                    { StatEnum.AttackRange, (typeof(float), 1f) }
                }
            });
            presenter.UpdateView().Forget();
        }
        public override void Initialize() { }
    }
}