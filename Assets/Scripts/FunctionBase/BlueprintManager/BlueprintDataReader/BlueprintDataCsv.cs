namespace FunctionBase.BlueprintManager.BlueprintDataReader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using FunctionBase.BlueprintManager.BlueprintBase;
    using FunctionBase.Extensions;

    public abstract class BlueprintDataCsv<TKey, TData> : IBlueprintData
    {
        public Dictionary<TKey, TData> Data;
        public void ConvertData(string rawData)
        {
            this.Data ??= new Dictionary<TKey, TData>();
            var dataRecord                   = CsvDataConverter.Deserialize<TData>(rawData);
            foreach (var record in dataRecord)
            {
                var    listRecord = record.GetFieldInfoWithAttribute<KeyOfRecord>();
                switch (listRecord.Count)
                {
                    case 0: throw new Exception("");
                    case 1:
                        if (listRecord.First().GetValue(record) != null)
                        {
                            this.Data.Add((TKey) listRecord.First().GetValue(record),record);
                        }
                        break;
                    case >1:
                        var multiKey = "";
                        listRecord.ForEach(field=>multiKey+=field.GetValue(record));
                        object key = multiKey;
                        this.Data.Add((TKey)key,record);
                        break;
                }
            }
        }

       
    }
}