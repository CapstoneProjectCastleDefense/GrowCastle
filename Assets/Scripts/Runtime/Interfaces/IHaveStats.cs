namespace Runtime.Interfaces
{
    using System;
    using System.Collections.Generic;
    using Runtime.Enums;
    using Runtime.Extensions;

    public interface IHaveStats
    {
        public Dictionary<StatEnum, (Type, object)> GetStats();
        public void                                 UpdateStats();
    }

    public interface IHaveStatsModel
    {
        Dictionary<StatEnum, (Type, object)> Stats { get; set; }
        
        public static IHaveStats operator +(IHaveStats a, IHaveStats b)
        {
            foreach (var stat in b.Stats)
            {
                a.SetStat(stat.Key, stat.Value.Item2);
            }

            return a;
        }
    }
}