namespace Models.LocalData.LocalDataController
{
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

        public void ReceiveResource(ResourceType resourceType, float receiveValue)
        {
            this.resourceLocalData.resource[resourceType] += receiveValue;
        }

        public bool SpendResource(ResourceType resourceType,float spendValue)
        {
            if(!this.CheckCanSpend(resourceType,spendValue)) return false;
            this.resourceLocalData.resource[resourceType] -= spendValue;

            return true;
        }

        public bool CheckCanSpend(ResourceType resourceType, float spendValue)
        {
            return this.resourceLocalData.resource[resourceType] >= spendValue;
        }
    }
}