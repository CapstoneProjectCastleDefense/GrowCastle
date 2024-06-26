﻿namespace Models.LocalData.LocalDataController
{
    using System.Collections.Generic;

    public class HeroLocalData : ILocalDataHaveController<HeroLocalDataController>
    {
        public List<HeroData> listHeroData = new();
        public void Init()
        {
        }
    }

    public class HeroData
    {
        public string     id;
        public int        level;
        public Status heroStatus;
    }

    public enum Status
    {
        Equip,
        UnLock,
        Lock,
    }

}