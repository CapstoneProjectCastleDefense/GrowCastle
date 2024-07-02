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
        }

        public ReactiveProperty<float> GetResource(ResourceType resourceType) => this.resourceLocalData.resource[resourceType];

        public void ReceiveResource(ResourceType resourceType, float receiveValue)
        {
            this.resourceLocalData.resource[resourceType].Value += receiveValue;
        }

        public bool SpendResource(ResourceType resourceType,float spendValue)
        {
            if(!this.CheckCanSpend(resourceType,spendValue)) return false;
            this.resourceLocalData.resource[resourceType].Value -= spendValue;

            return true;
        }

        public bool CheckCanSpend(ResourceType resourceType, float spendValue)
        {
            return this.resourceLocalData.resource[resourceType].Value >= spendValue;
        }
    }
}