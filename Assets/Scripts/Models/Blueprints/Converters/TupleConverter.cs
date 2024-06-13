namespace Models.Blueprints.Converters
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using BlueprintFlow.BlueprintReader.Converter;
    using BlueprintFlow.BlueprintReader.Converter.TypeConversion;
    using GameFoundation.Scripts.Utilities.Extension;

    public sealed class TupleConverter : ITypeConverter
    {
        private readonly string separator;

        public TupleConverter(string separator = "|") { this.separator = separator; }

        public object ConvertFromString(string str, Type type)
        {
            var items     = str.Split(new[] { this.separator }, StringSplitOptions.None);
            var itemTypes = type.GetGenericArguments();
            return Activator.CreateInstance(type, IterTools.StrictZip(items, itemTypes, (item, type) => CsvHelper.GetTypeConverter(type).ConvertFromString(item, type)).ToArray());
        }

        public string ConvertToString(object obj, Type type)
        {
            var tuple     = (ITuple)(obj);
            var itemTypes = type.GetGenericArguments();
            return string.Join(this.separator, IterTools.StrictZip(tuple.ToEnumerable(), itemTypes, (item, type) => CsvHelper.GetTypeConverter(type).ConvertToString(item, type)));
        }
    }
}