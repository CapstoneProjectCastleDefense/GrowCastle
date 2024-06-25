﻿namespace Runtime.Managers
{
    using System.Collections.Generic;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using Runtime.Elements.Base;
    using Runtime.Elements.Entities.MapLevel;
    using Runtime.Managers.Base;

    public class MapLevelManager : BaseElementManager<MapLevelModel,MapLevelPresenter,MapLevelView>
    {
        private readonly LevelLocalDataController levelLocalDataController;
        private          MapLevelPresenter        currentMapLevel;
        public MapLevelManager(BaseElementPresenter<MapLevelModel, MapLevelView, MapLevelPresenter>.Factory factory, LevelLocalDataController levelLocalDataController)
            : base(factory)
        {
            this.levelLocalDataController = levelLocalDataController;
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
            this.currentMapLevel.UpdateView().Forget();
            this.CreateEnvironmentInternal(model.LevelRecord.LevelToWaveRecords.First());
            return this.currentMapLevel;
        }
        private async void CreateEnvironmentInternal(KeyValuePair<int, LevelToWaveRecord> environment)
        {
            //await UniTask.Delay(TimeSpan.FromSeconds(environment.Value.Delay));
            this.currentMapLevel.SpawnEnvironment(environment.Key);
        }
    }
}