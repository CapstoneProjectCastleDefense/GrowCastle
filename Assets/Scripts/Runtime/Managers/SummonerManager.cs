namespace Runtime.Managers
{
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.Summoner;
    using Runtime.Managers.Base;

    public class SummonerManager : BaseElementManager<SummonerModel,SummonerPresenter,SummonerView>
    {
        public SummonerManager(BaseElementPresenter<SummonerModel, SummonerView, SummonerPresenter>.Factory factory) : base(factory)
        {
        }

        public override void Initialize()
        {

        }

        public override void DisposeAllElement()
        {

        }
    }
}