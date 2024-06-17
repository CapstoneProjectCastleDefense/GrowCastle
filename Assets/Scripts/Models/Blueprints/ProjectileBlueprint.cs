namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("Projectile", true)]
    [CsvHeaderKey("Id")]
    public class ProjectileBlueprint : GenericBlueprintReaderByRow<string, ProjectileRecord>
    {
    }

    public class ProjectileRecord
    {
        public string Id              { get; set; }
        public int    Fragment        { get; set; }
        public float  ProjectileSpeed { get; set; }
        public float  Delay           { get; set; }
    }
}