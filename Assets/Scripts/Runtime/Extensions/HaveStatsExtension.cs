namespace Runtime.Extensions
{
    using System;
    using System.Collections.Generic;
    using Runtime.Enums;
    using UnityEngine;
    using Runtime.Interfaces;
    using Sirenix.Utilities;

    public static class HaveStatsExtension
    {
        public static void SetStat<T>(this IHaveStatsModel haveStats, StatEnum statEnum, T value) { haveStats.Stats[statEnum] = (typeof(T), value); }

        public static T GetStat<T>(this IHaveStatsModel haveStats, StatEnum statEnum)
        {
            haveStats.Stats.TryGetValue(statEnum, out var value);
            if (value.Item1 == null)
            {
                return default;
            }

            if (!value.Item1.IsCastableTo(typeof(T)))
            {
                Debug.LogError($"[{nameof(HaveStatsExtension)}]: Cannot cast {value.Item1} to {typeof(T)}");
                return default;
            }

            return (T)haveStats.Stats[statEnum].Item2;
        }
        public static T ToEnum<T>(this string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }
        
        public static void PlusStat(this IHaveStatsModel haveStats, StatEnum statEnum, (Type, object) value)
        {
            var currentValue = haveStats.GetStat<object>(statEnum);
            if (currentValue is int intValue)
            {
                haveStats.SetStat(statEnum, intValue + (int)value.Item2);
            }
            else if (currentValue is float floatValue)
            {
                haveStats.SetStat(statEnum, floatValue + (float)value.Item2);
            }
            else
            {
                Debug.LogError($"[{nameof(HaveStatsExtension)}]: Cannot add {value.Item1} to {currentValue}");
            }
        }
        
        public static void MinusStat(this IHaveStatsModel haveStats, StatEnum statEnum, (Type, object) value)
        {
            var currentValue = haveStats.GetStat<object>(statEnum);
            if (currentValue is int intValue)
            {
                haveStats.SetStat(statEnum, intValue - (int)value.Item2);
            }
            else if (currentValue is float floatValue)
            {
                haveStats.SetStat(statEnum, floatValue - (float)value.Item2);
            }
            else
            {
                Debug.LogError($"[{nameof(HaveStatsExtension)}]: Cannot subtract {value.Item1} from {currentValue}");
            }
        }
        
        public static void Plus(this IHaveStatsModel haveStats, IHaveStatsModel otherStats)
        {
            otherStats.Stats.ForEach(stat => haveStats.PlusStat(stat.Key, stat.Value));
        }
        
        public static void Minus(this IHaveStatsModel haveStats, IHaveStatsModel otherStats)
        {
            otherStats.Stats.ForEach(stat => haveStats.MinusStat(stat.Key, stat.Value));
        }
        

        public static T GetStat<T>(this Dictionary<StatEnum, (Type, object)> stats, StatEnum statEnum)
        {
            stats.TryGetValue(statEnum, out var value);
            if (value.Item1 == null)
            {
                return default;
            }

            if (value.Item1 != typeof(T))
            {
                Debug.LogError($"[{nameof(HaveStatsExtension)}]: Cannot cast {value.Item1} to {typeof(T)}");
                return default;
            }

            return (T)stats[statEnum].Item2;
        }

        public static void SetStat<T>(this Dictionary<StatEnum, (Type, object)> stats, StatEnum statEnum, T value)
        {
            stats[statEnum] = (typeof(T), value);
        }
    }
}