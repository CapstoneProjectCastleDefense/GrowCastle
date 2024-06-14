namespace Models.Blueprints
{
    using System.Collections.Generic;
    using BlueprintFlow.BlueprintReader;

    [BlueprintReader("Hero", true)]
    [CsvHeaderKey("HeroId")]
    public class HeroBlueprint : GenericBlueprintReaderByRow<string,HeroRecord>
    {

    }

    public class HeroRecord
    {
        public string       HeroId      { get; set; }
        public string       PrefabName  { get; set; }
        public HeroClass    Class       { get; set; }
        public string       EvolutionId { get; set; }
        public float        BaseAttack  { get; set; }
        public List<string> Skill       { get; set; }
    }

    public enum HeroClass
    {
        Summon,
    }
}