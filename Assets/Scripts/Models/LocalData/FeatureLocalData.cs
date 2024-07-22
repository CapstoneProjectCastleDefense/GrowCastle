namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.Blueprints;
    using Models.LocalData.LocalDataController;
    using R3;

    public class FeatureLocalData : ILocalDataHaveController<FeatureLocalDataController>
    {
        public Dictionary<FeatureName, ReactiveProperty<bool>> FeatureData = new();
        public void Init()
        {
            
        }
    }
}