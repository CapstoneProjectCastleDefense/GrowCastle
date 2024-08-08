namespace Models.LocalData.LocalDataController
{
    using System;
    using R3;
    using Runtime.Enums;
    using Runtime.Services;

    public class ResourceLocalDataController : ILocalDataController
    {
        private readonly ResourceLocalData       resourceLocalData;
        private readonly UserLocalDataController userLocalDataController;
        private readonly InternetService         internetService;

        public ResourceLocalDataController(ResourceLocalData resourceLocalData, UserLocalDataController userLocalDataController, InternetService internetService)
        {
            this.resourceLocalData       = resourceLocalData;
            this.userLocalDataController = userLocalDataController;
            this.internetService         = internetService;
        }

        public void InitData()
        {
            this.resourceLocalData.Resource[ResourceType.Exp].Subscribe(this.OnUpdateExp);
            var totalDiffDay = this.internetService.ToTalDiffDay(DateTime.Now, this.resourceLocalData.LastDate);
            this.ReceiveResource(ResourceType.Ticket,totalDiffDay);
            this.resourceLocalData.LastDate = DateTime.Now;
        }

        public float                   GetCurrentTargetExpToLevelUp()         => this.resourceLocalData.CurrentTargetExpToLevelUp.Value;
        public ReactiveProperty<float> GetResource(ResourceType resourceType) => this.resourceLocalData.Resource[resourceType];

        public void ReceiveResource(ResourceType resourceType, float receiveValue)
        {
            if(!this.resourceLocalData.Resource.ContainsKey(resourceType)) return;
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
            this.userLocalDataController.UpgradeUserLevel();
        }
    }
}