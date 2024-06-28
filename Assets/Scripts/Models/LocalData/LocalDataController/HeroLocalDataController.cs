namespace Models.LocalData.LocalDataController
{
    using Models.Blueprints;
    using Sirenix.Utilities;

    public class HeroLocalDataController : ILocalDataController
    {
        private readonly HeroLocalData heroLocalData;
        private readonly HeroBlueprint heroBlueprint;

        public HeroLocalDataController(HeroLocalData heroLocalData, HeroBlueprint heroBlueprint)
        {
            this.heroLocalData = heroLocalData;
            this.heroBlueprint = heroBlueprint;
        }

        public void InitData()
        {
            if (this.heroLocalData.listHeroData.Count == 0)
            {
                this.heroLocalData.listHeroData = new();
                this.heroBlueprint.ForEach(hero => { this.heroLocalData.listHeroData.Add(new() { id = hero.Key, heroStatus = HeroStatus.Lock, level = 1 }); });
                this.heroLocalData.listHeroData[0].heroStatus = HeroStatus.UnLock;
            }
        }
    }
}