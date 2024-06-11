namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;
    using BlueprintFlow.BlueprintReader.Converter;
    using Models.Blueprints.Converters;

    [BlueprintReader("Enemy", true)]
    [CsvHeaderKey("Id")]
    public class EnemyBlueprint : GenericBlueprintReaderByRow<string, EnemyRecord>
    {
        static EnemyBlueprint() { CsvHelper.RegisterTypeConverter(typeof((int, int)), new TupleConverter()); }
    }

    public class EnemyRecord
    {
        public string                               Id         { get; set; }
        public string                               PrefabName { get; set; }
        public AttackType                           AttackType { get; set; }
        public string                               Name       { get; set; }
        public (float baseValue, float coefficient) Attack     { get; set; }
        public (float baseValue, float coefficient) HP         { get; set; }
        public (float baseValue, float coefficient) Speed      { get; set; }
    }

    public enum AttackType
    {
        Melee
    }
}