namespace Runtime.Elements.Entities.Summoner
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using UnityEngine;

    public class SummonerPresenter : BaseElementPresenter<SummonerModel,SummonerView,SummonerPresenter>
    {
        public SummonerPresenter(SummonerModel model, ObjectPoolManager objectPoolManager) : base(model, objectPoolManager)
        {
        }

        protected override UniTask<GameObject> CreateView()
        {
            return this.ObjectPoolManager.Spawn(this.Model.AddressableName);
        }

        public override void Dispose()
        {

        }
    }

    public class SummonerModel : IElementModel
    {
        public string Id              { get; set; }
        public string AddressableName { get; set; }
    }
}