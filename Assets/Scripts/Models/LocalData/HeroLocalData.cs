namespace Models.LocalData
{
    using System.Collections.Generic;
    using Models.LocalData.LocalDataController;
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
        public ReactiveProperty<HeroStatus> HeroStatus      { get; set; } = new(LocalData.HeroStatus.Lock);
    }

    public enum HeroStatus
    {
        Equip,
        UnLock,
        Lock,
    }
}