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
        public static void SetStat<T>(this IHaveStats haveStats, StatEnum statEnum, T value) { haveStats.Stats[statEnum] = (typeof(T), value); }

        public static T GetStat<T>(this IHaveStats haveStats, StatEnum statEnum)
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