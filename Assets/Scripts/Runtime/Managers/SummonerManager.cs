namespace Runtime.Managers
{
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Summoner;
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
            var presenter      = this.CreateElement(new SummonerModel() { Id = summonerId, AddressableName = summonerRecord.PrefabName, StartPos = startPos, SortingIndex = sortingIndex });
            presenter.UpdateView().Forget();
        }
        public override void Initialize() { }
    }
}