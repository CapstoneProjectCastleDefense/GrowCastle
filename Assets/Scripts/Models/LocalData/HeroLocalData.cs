namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using R3;

    public class HeroLocalData : ILocalDataHaveController<HeroLocalDataController>
    {
        public List<HeroData> listHeroData = new();
        public void Init()
        {
        }
    }

    public class HeroData
    {
        public string                       id;
        public int                          level;
        public ReactiveProperty<HeroStatus> HeroStatus { get; set; } = new(LocalDataController.HeroStatus.Lock);
    }

    public enum HeroStatus
    {
        Equip,
        UnLock,
        Lock,
    }

}