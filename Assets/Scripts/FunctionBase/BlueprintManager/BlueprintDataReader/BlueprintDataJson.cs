namespace FunctionBase.BlueprintManager.BlueprintDataReader
{
    using System.Collections.Generic;
    using FunctionBase.BlueprintManager.BlueprintBase;

    public abstract class BlueprintDataJson<TKey, TData> : IBlueprintData where TData : class
    {
        public Dictionary<TKey, TData> Data;
        public void                    ConvertData(string rawData)            { }
    }
}