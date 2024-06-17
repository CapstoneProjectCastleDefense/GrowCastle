namespace Runtime.Interfaces
{
    using System;
    using Runtime.Enums;
    using UnityEngine;

    public static class HaveStatsExtension
    {
        public static void SetStat<T>(this IHaveStats haveStats, StatEnum statEnum, T value) { haveStats.Stats[statEnum] = (typeof(T), value); }

        public static T GetStat<T>(this IHaveStats haveStats, StatEnum statEnum)
        {
            haveStats.Stats.TryGetValue(statEnum, out var value);
            if (value.Item1 == null)
            {
                Debug.LogError($"[{nameof(HaveStatsExtension)}]: Stat {statEnum} not found");
                return default;
            }

            if (value.Item1 != typeof(T))
            {
                Debug.LogError($"[{nameof(HaveStatsExtension)}]: Cannot cast {value.Item1} to {typeof(T)}");
                return default;
            }

            return (T)haveStats.Stats[statEnum].Item2;
        }
    }
}