namespace Models.LocalData.LocalDataController
{
    using GameFoundation.Scripts.Utilities.Extension;
    using Models.Blueprints;
    using R3;

    public class FeatureLocalDataController : ILocalDataController
    {
        private readonly FeatureLocalData featureLocalData;
        private readonly FeatureBlueprint featureBlueprint;
        public FeatureLocalDataController(FeatureLocalData featureLocalData, FeatureBlueprint featureBlueprint)
        {
            this.featureLocalData = featureLocalData;
            this.featureBlueprint = featureBlueprint;
        }
        public void InitData()
        {
            if (this.featureLocalData.FeatureData.Count == 0)
            {
                this.featureBlueprint.ForEach(data =>
                {
                    this.featureLocalData.FeatureData.Add(data.Key,new ReactiveProperty<bool>(false));
                });
            }
        }
        public ReactiveProperty<bool> GetFeatureData(FeatureName featureName) => this.featureLocalData.FeatureData[featureName];
        
        public bool CheckFeatureIsUnlock(FeatureName featureName,int value)
        {
            var result = value >= this.featureBlueprint[featureName].TargetValueToUnlock;
            if (result)
            {
                this.featureLocalData.FeatureData[featureName].Value = true;
            }
            return value >= this.featureBlueprint[featureName].TargetValueToUnlock;
        }
    }
}