namespace Models.Blueprints
{
    public class ElementUpgradeBlueprint
    {
        
    }

    public class ElementUpgradeRecord
    {
        public string Id                       { get; set; }
        public float  BaseCost                 { get; set; }
        public int    LevelInterval            { get; set; }
        public float  CostChangePerInterval    { get; set; }
        public float  AttackEnhancePerInterval { get; set; }
        public int    MaxLevel                 { get; set; }
    }
}