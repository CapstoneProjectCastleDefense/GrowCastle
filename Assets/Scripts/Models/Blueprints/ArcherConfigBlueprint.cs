namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("ArcherConfig", true)]
    public class ArcherConfigBlueprint: GenericBlueprintReaderByCol
    {
        public float  BaseDamage      { get; set; }
        public float  BaseAttackSpeed { get; set; }
        public float  Coefficient     { get; set; }
        public string Recipe          { get; set; }
        public float  BaseGold        { get; set; }

    }
}