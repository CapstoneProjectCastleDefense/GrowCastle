namespace FunctionBase.BlueprintManager.BlueprintBase
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class DataInfoAttribute : Attribute
    {
        public readonly string DataPath;
        public DataInfoAttribute(string dataPath)
        {
            this.DataPath            = dataPath;
        }
    }
}