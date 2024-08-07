namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using BlueprintFlow.BlueprintReader.Converter;
    using Models.Blueprints.Converters;
    using Runtime.Enums;

    [BlueprintReader("ItemRarityStat", true)] [CsvHeaderKey("ItemRarity")]
    public class ItemRarityStatBlueprint : GenericBlueprintReaderByRow<RarityEnum, ItemRarityStatRecord>
    {
        static ItemRarityStatBlueprint() { CsvHelper.RegisterTypeConverter(typeof((float, float)), new TupleConverter()); }
    }

    public class ItemRarityStatRecord
    {
        public RarityEnum                                ItemRarity        { get; set; }
        public BlueprintByRow<StatEnum,ItemRarityToStat> ItemRarityToStats { get; set; }
        
    }

    [CsvHeaderKey("ItemStat")]
    public class ItemRarityToStat
    {
        public StatEnum                         ItemStat       { get; set; }
        public (float minValue, float maxValue) StatRangeValue { get; set; }
    }
}