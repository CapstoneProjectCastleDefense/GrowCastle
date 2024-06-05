namespace Runtime.Elements.Entities.MapLevel
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Runtime.Elements.Base;
    using UnityEngine;

    public class MapLevelPresenter : BaseElementPresenter<MapLevelModel,MapLevelView,MapLevelPresenter>
    {
        public MapLevelPresenter(MapLevelModel model, ObjectPoolManager objectPoolManager)
            : base(model, objectPoolManager)
        {
        }
        public override void OnDestroyPresenter()
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

    public class MapLevelModel : IElementModel
    {
        public string Id              { get; set; }
        public string AddressableName { get; set; }
    }
}