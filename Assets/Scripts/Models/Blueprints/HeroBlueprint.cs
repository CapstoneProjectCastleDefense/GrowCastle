namespace Models.Blueprints
{
    using System.Collections.Generic;
    using BlueprintFlow.BlueprintReader;
    using BlueprintFlow.BlueprintReader.Converter;
    using Models.Blueprints.Converters;
    using Models.LocalData;

    [BlueprintReader("Hero", true)] [CsvHeaderKey("HeroId")]
    public class HeroBlueprint : GenericBlueprintReaderByRow<string, HeroRecord>
    {
        static HeroBlueprint() { CsvHelper.RegisterTypeConverter(typeof((string, string)), new TupleConverter()); }
    }

    public class HeroRecord
    {
        public string                                   HeroId            { get; set; }
        public string                                   PrefabName        { get; set; }
        public HeroClass                                Class             { get; set; }
        public string                                   EvolutionId       { get; set; }
        public string                                   SkeletonDataAsset { get; set; }
        public SlotType                                 HeroType          { get; set; }
        public (string skillName, string animationName) ActiveSkill       { get; set; }
        public (string skillName, string animationName) AttackSkill       { get; set; }
        public List<string>                             PassiveSkill      { get; set; }
    }


    public enum HeroClass
    {
        Summon,
        Attack,
        Buff
    }
}