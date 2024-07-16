namespace Models.LocalData.LocalDataController
{
    using R3;

    public class ResourceLocalDataController : ILocalDataController
    {
        private readonly ResourceLocalData resourceLocalData;

        public ResourceLocalDataController(ResourceLocalData resourceLocalData)
        {
            this.resourceLocalData = resourceLocalData;
        }

        public void InitData()
        {
            this.resourceLocalData.Resource[ResourceType.Exp].Subscribe(this.OnUpdateExp);
        }

        public float                   GetCurrentTargetExpToLevelUp()         => this.resourceLocalData.CurrentTargetExpToLevelUp.Value;
        public ReactiveProperty<float> GetResource(ResourceType resourceType) => this.resourceLocalData.Resource[resourceType];

        public void ReceiveResource(ResourceType resourceType, float receiveValue)
        {
            this.resourceLocalData.Resource[resourceType].Value += receiveValue;
        }

        public bool SpendResource(ResourceType resourceType,float spendValue)
        {
            if(!this.CheckCanSpend(resourceType,spendValue)) return false;
            this.resourceLocalData.Resource[resourceType].Value -= spendValue;

            return true;
        }

        private bool CheckCanSpend(ResourceType resourceType, float spendValue)
        {
            return this.resourceLocalData.Resource[resourceType].Value >= spendValue;
        }

        private void OnUpdateExp(float expValue)
        {
            var currentTargetValue = this.resourceLocalData.CurrentTargetExpToLevelUp.Value;
            if (!(this.GetResource(ResourceType.Exp).Value >= this.resourceLocalData.CurrentTargetExpToLevelUp.Value)) return;
            this.ReceiveResource(ResourceType.TalentPoint,1);
            this.resourceLocalData.Resource[ResourceType.Exp].Value = 0;
            this.resourceLocalData.CurrentTargetExpToLevelUp.Value  = currentTargetValue * 1.5f;
        }
    }
}