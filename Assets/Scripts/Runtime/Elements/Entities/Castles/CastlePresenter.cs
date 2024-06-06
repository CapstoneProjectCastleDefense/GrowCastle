namespace Runtime.Elements.Entities.Castles
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using UnityEngine;

    public class CastlePresenter : BaseElementPresenter<CastleModel, CastleView, CastlePresenter>
    {
        public CastlePresenter(CastleModel model, ObjectPoolManager objectPoolManager)
            : base(model, objectPoolManager)
        {
        }
        public override    void                OnDestroyPresenter() { }
        protected override UniTask<GameObject> CreateView()         { return this.ObjectPoolManager.Spawn(this.Model.AddressableName); }

        public          void SpawnBlockBaseOnLevel(int level) { }
        public override void Dispose()                        { }
    }

    public class CastleModel : IElementModel
    {
        public string Id              { get; set; }
        public string AddressableName { get; set; }

        public CastleStat CastleStat { get; set; }
    }

    public class CastleStat
    {
        public int Mp            { get; set; }
        public int Hp            { get; set; }
        public int GoldToUpgrade { get; set; }
    }
}