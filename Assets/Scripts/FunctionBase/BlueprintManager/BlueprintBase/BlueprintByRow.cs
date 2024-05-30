namespace FunctionBase.BlueprintManager.BlueprintBase
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using FunctionBase.BlueprintManager.BlueprintDataReader;

    public class BlueprintByRow<TKey, TData> : Dictionary<TKey, TData>, IBlueprintByRow where TData : class, new()
    {
        public void ReadRecord(string[] rawData,Dictionary<string, int> table)
        {
            TKey key = default;
            var v = Activator.CreateInstance(typeof(TData));

            var fieldInfo = typeof(TData).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var tmp in fieldInfo)
            {
                if (!table.ContainsKey(tmp.Name)) continue;
                var idx = table[tmp.Name];
                if (idx < rawData.Length)
                    CsvDataConverter.SetValue(v, tmp, rawData[idx]);
                if (tmp.GetCustomAttribute<KeyOfRecord>() != null) key = (TKey)tmp.GetValue(v);
            }

            if (key == null) throw new Exception("This record not have key!");
            this.Add(key, (TData)v);
        }
    }

    public interface IBlueprintByRow
    {
        public void ReadRecord(string[] rawData,Dictionary<string, int> table);
    }
}