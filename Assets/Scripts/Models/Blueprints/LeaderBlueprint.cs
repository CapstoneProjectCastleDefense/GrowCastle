namespace Models.Blueprints
{
    using BlueprintFlow.BlueprintReader;

    public class LeaderBlueprint : GenericBlueprintReaderByRow<string, LeaderRecord>
    {
    }

    public class LeaderRecord
    {
        public string                                         LeaderId                { get; set; }
        public string                                         PrefabName              { get; set; }
        public string                                         EvolutionId             { get; set; }
        public float                                          BaseAttack              { get; set; }
        public float                                          BaseHealth              { get; set; }
        public float                                          BaseMoveSpeed           { get; set; }
        public float                                          AttackRange             { get; set; }
        public float                                          AttackSpeed             { get; set; }
        public BlueprintByRow<string, SkillToAnimationRecord> SkillToAnimationRecords { get; set; }
    }
}