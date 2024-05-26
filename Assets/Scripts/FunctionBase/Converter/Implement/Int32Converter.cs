namespace FunctionBase.Converter.Implement
{
    using System;
    using FunctionBase.Converter.BaseConvert;
    using FunctionBase.Extensions;

    public class Int32Converter : BaseConverter
    {
        protected override Type ConvertibleType => typeof(int);

        protected override object ConvertFromString_Internal(string str, Type type)
        {
            bool a = str.IsNullOrWhitespace();
            return int.Parse(str.IsNullOrWhitespace() ? "0" : str);
        }

        protected override string ConvertToString_Internal(object obj, Type type)
        {
            return obj?.ToString() ?? "0";
        }
    }
}