namespace Models.Blueprints
{
    public class LevelBlueprint
    {
        
    }

    public class LevelRecord
    {
        public int    Id            { get; set; }
        public string WaveId        { get; set; }
        public float  Delay         { get; set; }
        public string EnvironmentId { get; set; }
    }
}