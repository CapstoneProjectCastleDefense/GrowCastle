namespace Runtime.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Runtime.Enums;

    public interface IHaveStats
    {
        public Dictionary<StatEnum, (Type, object)> GetStats();
        public void                                 UpdateStats();
    }

    public interface IHaveStatsModel
    {
        Dictionary<StatEnum, (Type, object)> Stats { get; set; }
    }
}