namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using R3;

    public class HeroLocalData : ILocalDataHaveController<HeroLocalDataController>
    {
        public Dictionary<string, HeroData> IdToHeroData { get; set; } = new();
        public void                         Init()       { }
    }

    public class HeroData
    {
        public string                       Id              { get; set; }
        public int                          Level           { get; set; }
        public List<string>                 ListEquipmentId { get; set; }
        public ReactiveProperty<HeroStatus> HeroStatus      { get; set; } = new(LocalDataController.HeroStatus.Lock);
    }

    public enum HeroStatus
    {
        Equip,
        UnLock,
        Lock,
    }
}