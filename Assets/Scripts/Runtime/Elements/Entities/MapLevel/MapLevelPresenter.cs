namespace Runtime.Elements.Entities.MapLevel
{
    using Cysharp.Threading.Tasks;
    using GameFoundation.Scripts.Utilities.ObjectPool;
    using Models.Blueprints;
    using Runtime.Elements.Base;
    using UnityEngine;

    public class MapLevelPresenter : BaseElementPresenter<MapLevelModel, MapLevelView, MapLevelPresenter>
    {
        private readonly EnvironmentBlueprint environmentBlueprint;
        private          GameObject           currentEnvi;
        public MapLevelPresenter(MapLevelModel model, ObjectPoolManager objectPoolManager, EnvironmentBlueprint environmentBlueprint)
            : base(model, objectPoolManager)
        {
            this.environmentBlueprint = environmentBlueprint;
        }
        protected override UniTask<GameObject> CreateView()         { return this.ObjectPoolManager.Spawn(this.Model.LevelRecord.PrefabName); }

        public override async UniTask UpdateView()
        {
            await base.UpdateView();
            this.View.transform.position = Vector3.zero;
        }

        public async void SpawnEnvironment(int waveId)
        {
            await UniTask.WaitUntil(() => this.View != null);
            if (this.currentEnvi != null) Object.Destroy(this.currentEnvi);
            var environmentPrefabName = this.environmentBlueprint.GetDataById(this.Model.LevelRecord.LevelToWaveRecords[waveId].EnvironmentId).PrefabName;
            this.currentEnvi                    = await this.ObjectPoolManager.Spawn(environmentPrefabName, this.View.transform);
            this.currentEnvi.transform.position = this.View.environmentPos.position;
        }
        public override void Dispose()
        {
            if (this.currentEnvi != null) Object.Destroy(this.currentEnvi);
            this.View.Recycle();
        }
    }

    public class MapLevelModel : IElementModel
    {
        public string      Id              { get; set; }
        public string      AddressableName { get; set; }
        public LevelRecord LevelRecord     { get; set; }
        public string      CurrentWaveId   { get; set; }
    }
}