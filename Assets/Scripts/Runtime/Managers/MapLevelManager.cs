namespace Runtime.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Models.LocalData;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.MapLevel;
    using Runtime.Managers.Base;

    public class MapLevelManager : BaseElementManager<MapLevelModel,MapLevelPresenter,MapLevelView>
    {
        private readonly LevelLocalData    levelLocalData;
        private          MapLevelPresenter currentMapLevel;
        public MapLevelManager(BaseElementPresenter<MapLevelModel, MapLevelView, MapLevelPresenter>.Factory factory, LevelLocalData levelLocalData)
            : base(factory)
        {
            this.levelLocalData = levelLocalData;
        }
        public override void Initialize()
        {
            
        }

        public override MapLevelPresenter CreateElement(MapLevelModel model)
        {
            if (this.currentMapLevel != null)
            {
                this.currentMapLevel.Dispose();
                this.currentMapLevel = null;
            }
            this.currentMapLevel = this.Factory.Create(model);
            this.CreateEnvironmentInternal(model.LevelRecord.LevelToWaveRecords.First());
            return this.currentMapLevel;
        }
        private async void CreateEnvironmentInternal(KeyValuePair<string, LevelToWaveRecord> environment)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(environment.Value.Delay));
            this.currentMapLevel.SpawnEnvironment(environment.Key);
        }
        public override void DisposeAllElement()
        { 
            
        }
    }
}